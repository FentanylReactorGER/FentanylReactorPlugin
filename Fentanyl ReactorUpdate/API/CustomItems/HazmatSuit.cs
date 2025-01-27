using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Toys;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;
using UnityEngine.UIElements;
using Light = Exiled.API.Features.Toys.Light;

namespace Fentanyl_ReactorUpdate.API.CustomItems;

[CustomItem(ItemType.ArmorHeavy)]
public class HazmatSuit : CustomItem
{
    private static readonly Config Config = Plugin.Singleton.Config;
    private static readonly Translation Translation = Plugin.Singleton.Translation;
    public override string Name { get; set; } = "Strahlenanzug";
    public override string Description { get; set; } = "Schützt flächenweit vor SCP-1356!";
    public override float Weight { get; set; } = Config.T1Weight;
    public override uint Id { get; set; } = 6912;
    public override SpawnProperties SpawnProperties { get; set; }
    private List<Player> HazmatDropper { get; set; } = new();
    private List<CustomItem> HazmatPickupsDrop { get; set; } = new();
    private List<Player> HazmatPickers { get; set; } = new();
    private List<Pickup> HazmatPickups { get; set; } = new();
    private List<Player> HazamtSuitWears { get; set; } = new();
    
    private Dictionary<Pickup, SchematicObject> ActiveBreads { get; set; } = new();
    private Dictionary<Pickup, Light> ActiveLights { get; set; } = new();

    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.DroppingItem += DroppedItem;
        Exiled.Events.Handlers.Player.SearchingPickup += PickingUp;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {

        Exiled.Events.Handlers.Player.SearchingPickup -= PickingUp;
        Exiled.Events.Handlers.Player.DroppingItem -= DroppedItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
        base.UnsubscribeEvents();
    }

    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (ev.Player.IsAlive && TryGet(ev.Player, out CustomItem Custom) && Custom?.Id == 6912)
        {
            ev.Player.DisableEffect<Slowness>();
            HazamtSuitWears.Remove(ev.Player);
            HazmatDropper.Remove(ev.Player);
            HazmatPickers.Remove(ev.Player);
            HazamtSuitWears.Remove(ev.Player);
        }
    }
    
    private IEnumerator<float> EnsureDroppedItemLights(Player player, Pickup pickup)
    {
        while (player != null && player.IsAlive)
        {
            if (HazamtSuitWears.Contains(player))
            {
                bool hasItem = false;

                foreach (var item in player.Items)
                {
                    if (CustomItem.TryGet(item, out CustomItem customItem) && customItem.Id == 6912)
                    {
                        hasItem = true;
                        break;
                    }
                }

                if (!hasItem)
                {
                    Timing.RunCoroutine(SpawnBreadSchematicAndLight(pickup));
                    HazamtSuitWears.Remove(player);
                    Log.Info($"Player {player.Nickname} removed from HazmatSuitWears as they no longer have the correct item.");
                }
            }

            yield return Timing.WaitForOneFrame;
        }
    }

    
    private void OnRoundEnded(RoundEndedEventArgs ev)
    {
        HazamtSuitWears.Clear();
        HazmatDropper.Clear();
        HazmatPickupsDrop.Clear();
        HazmatPickers.Clear();
        HazmatPickups.Clear();
        HazamtSuitWears.Clear();
    }
    
    private void DroppedItem(DroppingItemEventArgs ev)
    {
        if (Check(ev.Item) && ev.Item != null)
        {
            ev.IsAllowed = false;
            Timing.RunCoroutine(HintCounterRemove(ev));
        }
    }

    private void PickingUp(SearchingPickupEventArgs ev)
    {
        if (Check(ev.Player))
        {
            if (ev.Pickup.Base.ItemId.TypeId == ItemType.ArmorHeavy || ev.Pickup.Base.ItemId.TypeId == ItemType.ArmorLight || ev.Pickup.Base.ItemId.TypeId == ItemType.ArmorCombat)
            {
                ev.IsAllowed = false;
                ev.Player.ShowMeowHint("Du hast bereits den Hazmat Suit!");
            }
        }
        if (Check(ev.Pickup) && ev.Pickup != null)
        {
            ev.IsAllowed = false;
            Timing.CallDelayed(10, () => { Timing.RunCoroutine(EnsureDroppedItemLights(ev.Player, ev.Pickup)); });
            Timing.RunCoroutine(HintCounterGive(ev));
        }
    }

    private IEnumerator<float> HintCounterGive(SearchingPickupEventArgs ev)
    {
        while (ev.Player.IsAlive && ev.Player.IsConnected)
        {
            if (HazmatPickers.Contains(ev.Player))
            {
                ev.Player.ShowMeowHintDur("Du sammelst diesen Strahlenanzug bereits auf!", 1);
                yield break;
            }
            
            if (HazamtSuitWears.Contains(ev.Player))
            {
                ev.Player.ShowMeowHintDur("Du trägst bereits ein Strahlenanzug!", 1);
                yield break;
            }

            if (HazmatPickups.Contains(ev.Pickup))
            {
                ev.Player.ShowMeowHintDur("Dieser Strahlenanzug wird gerade von jemand anderem aufgesammelt!", 1);
                yield break;
            }
            ev.Player.Position.SpecialPosExtra("PickHazmat.ogg", 2, 3, 5);
            HazamtSuitWears.Add(ev.Player);
            HazmatPickers.Add(ev.Player);
            HazmatPickups.Add(ev.Pickup);

            string[] progress =
            {
                "<color=#ff7400>\u2b1b</color><color=#5a5a5a>\u2b1b\u2b1b\u2b1b\u2b1b</color> - Aufnehmen gestartet...",
                "<color=#ff7400>\u2b1b\u2b1b</color><color=#5a5a5a>\u2b1b\u2b1b\u2b1b</color> - Bitte warten...",
                "<color=#ff7400>\u2b1b\u2b1b\u2b1b</color><color=#5a5a5a>\u2b1b\u2b1b</color> - Gleich geschafft...",
                "<color=#ff7400>\u2b1b\u2b1b\u2b1b\u2b1b</color><color=#5a5a5a>\u2b1b</color> - Fast fertig...",
                "<color=#ff7400>\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b</color> - Aufnehmen abgeschlossen!"
            };

            ev.Player.EnableEffect<Ensnared>();
            foreach (string step in progress)
            {
                ev.Player.ShowMeowHintDur(step, 1);
                yield return Timing.WaitForSeconds(1f);
            }

            ev.Player.ShowMeowHintDur(
                "<b><color=green>✔️ Du hast den Strahlenanzug erfolgreich aufgesetzt!</color></b>", 2);
            ev.Player.Position.SpecialPosExtra("PickHazmat.ogg", 2, 3, 5);
            ev.Pickup.Destroy();
            TryGive(ev.Player, 6912);
            
            if (!ActiveBreads.TryGetValue(ev.Pickup, out var breadSchematic))
                yield break;
            
            breadSchematic.Destroy();
            ActiveBreads.Remove(ev.Pickup);

            if (ActiveLights.TryGetValue(ev.Pickup, out var light))
            {
                light.Destroy();
                ActiveLights.Remove(ev.Pickup);
            }
            
            ev.IsAllowed = true;
            HazmatPickers.Remove(ev.Player);
            HazmatPickups.Remove(ev.Pickup);
            ev.Player.DisableEffect<Ensnared>();
            ev.Player.EnableEffect<Slowness>();

            yield break;
        }
    }

    private IEnumerator<float> HintCounterRemove(DroppingItemEventArgs ev)
    {
        while (ev.Player.IsAlive && ev.Player.IsConnected)
        {
            if (HazmatDropper.Contains(ev.Player))
            {
                ev.Player.ShowMeowHintDur("Du legst diesen Strahlenanzug bereits ab!", 1);
                yield break;
            }

            if (HazmatPickupsDrop.Contains(Get(6912)))
            {
                ev.Player.ShowMeowHintDur("Dieser Strahlenanzug wird gerade von jemand anderem abgelegt!", 1);
                yield break;
            }
            ev.Player.Position.SpecialPosExtra("PickHazmat.ogg", 2, 3, 5);
            HazmatDropper.Add(ev.Player);
            HazmatPickupsDrop.Add(Get(6912));

            string[] progress =
            {
                "<color=#ff7400>\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b</color> - Ablegen gestartet...",
                "<color=#ff7400>\u2b1b\u2b1b\u2b1b\u2b1b</color><color=#5a5a5a>\u2b1b</color> - Bitte warten...",
                "<color=#ff7400>\u2b1b\u2b1b\u2b1b</color><color=#5a5a5a>\u2b1b\u2b1b</color> - Gleich geschafft...",
                "<color=#ff7400>\u2b1b\u2b1b</color><color=#5a5a5a>\u2b1b\u2b1b\u2b1b</color> - Fast fertig...",
                "<color=#ff7400>\u2b1b</color><color=#5a5a5a>\u2b1b\u2b1b\u2b1b\u2b1b</color> - Ablegen abgeschlossen!"
            };

            ev.Player.EnableEffect<Ensnared>();
            foreach (string step in progress)
            {
                ev.Player.ShowMeowHintDur(step, 1);
                yield return Timing.WaitForSeconds(1f);
            }

            ev.Player.ShowMeowHintDur("<b><color=red>❌ Du hast den Strahlenanzug abgelegt!</color></b>", 2);
            ev.Player.Position.SpecialPosExtra("PickHazmat.ogg", 2, 3, 5);
            var spawnedPickup = ev.Item.CreatePickup(ev.Player.Position);
            spawnedPickup.Scale = new Vector3(0.1f, 0.5f, 0.6f);
            Quaternion rotation = spawnedPickup.Rotation;
            rotation.eulerAngles = new Vector3(0, 0, 90);
            spawnedPickup.Rotation = rotation;
            Timing.RunCoroutine(SpawnBreadSchematicAndLight(spawnedPickup));
            ev.Item.Destroy();
            //if (TrySpawn(6912, ev.Player.Position, out spawnedPickup))
            //{
             //   ev.Item.Destroy();
           //     spawnedPickup?.Destroy();
         //   }
            ev.IsAllowed = true;
            HazmatDropper.Remove(ev.Player);
            HazmatPickupsDrop.Remove(Get((uint)6912));
            ev.Player.DisableEffect<Ensnared>();
            ev.Player.EnableEffect<Slowness>();
            HazamtSuitWears.Remove(ev.Player);
            yield break;
        }
    }
    private IEnumerator<float> SpawnBreadSchematicAndLight(Pickup pickup)
    {
        if (pickup == null)
            yield break;

        const string SchemeName = "GasMask";
        var breadSchematic = ObjectSpawner.SpawnSchematic(
            SchemeName,
            pickup.Position,
            Quaternion.Euler(pickup.Rotation.eulerAngles.x, pickup.Rotation.eulerAngles.y, 0),
            Vector3.one,
            MapUtils.GetSchematicDataByName(SchemeName),
            true);

        if (breadSchematic == null)
            yield break;

        Log.Info(
            $"Spawned GasMask schematic at {pickup.Position} with rotation {breadSchematic.transform.rotation.eulerAngles}.");
        ActiveBreads[pickup] = breadSchematic;

        var light = Exiled.API.Features.Toys.Light.Create(
            pickup.Position);
        light.Color = new Color(1f, 0.65f, 0f);
        light.Position = new Vector3(pickup.Position.x, pickup.Position.y + 0.9f, pickup.Position.z);
        light.Range = 5;
        light.Intensity = 3;
        light.ShadowType = LightShadows.Hard;
        ActiveLights[pickup] = light;

        if (light == null)
            yield break;

        ActiveLights[pickup] = light;

        while (pickup != null && pickup.GameObject != null && ActiveBreads.ContainsKey(pickup))
        {
            breadSchematic.transform.position = pickup.Position;
            var rotation = pickup.Rotation.eulerAngles;
            breadSchematic.transform.rotation = Quaternion.Euler(rotation.x, -rotation.y, 0);
            light.Position = pickup.Position;

            yield return Timing.WaitForOneFrame;
        }

        if (ActiveLights.TryGetValue(pickup, out var activeLight))
        {
            activeLight.Destroy();
            ActiveLights.Remove(pickup);
        }

        if (ActiveBreads.TryGetValue(pickup, out var activeBread))
        {
            activeBread.Destroy();
            ActiveBreads.Remove(pickup);
        }
    }
}