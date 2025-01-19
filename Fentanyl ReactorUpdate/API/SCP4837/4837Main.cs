using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.Commands.UtilityCommands;
using MapEditorReborn.Events.EventArgs;
using MEC;
using SSMenuSystem.Features;
using UnityEngine;
using UserSettings.ServerSpecific;
using Utils.Networking;
using Light = Exiled.API.Features.Toys.Light;

namespace Fentanyl_ReactorUpdate.API.SCP4837;

public class Main4837
{
    public SchematicObject SCP4837 { get; set; }
    public SchematicObject SCP4837TradeRoom { get; set; }
    public Vector3 SCP4837TradeRoomPlayerPos { get; set; }
    public Vector3 SCP4837TradeRoomPlayerPosOld { get; set; }
    private Transform Scp4837PickupPosition_1 { get; set; }
    private Transform Scp4837PickupPosition_2 { get; set; }
    private Transform Scp4837PickupPosition_3 { get; set; }
    private CustomItem SCP4837Custom { get; set; }
    private Transform Scp4837PickupPositionScheme { get; set; }
    private Vector3 Scp4837PickupPositionSchemeOld { get; set; }
    private Quaternion Scp4837PickupRotationSchemeOld { get; set; }
    private List<Pickup> Scp4837Pickups { get; set; } = new List<Pickup>();
    private List<CustomItem> Scp4837CustomItems { get; set; } = new List<CustomItem>();
    private Transform SCP4837Light { get; set; }
    private bool _hasPlayerPickedUp { get; set; }
    public bool _Cooldown { get; set; }
    private float Scp4837TradeDur = 30f;
    private List<ItemType> SCP4837TradeItems = new List<ItemType>
    {
        ItemType.KeycardO5,
        ItemType.GrenadeFlash,
        ItemType.Medkit,
        ItemType.Coin,
        ItemType.SCP500,
        ItemType.Adrenaline,
        ItemType.Flashlight,
        ItemType.Lantern,
        ItemType.Jailbird,
        ItemType.GunShotgun,
        ItemType.ParticleDisruptor,
        ItemType.Painkillers,
        ItemType.SCP207,
        ItemType.SCP1344,
        ItemType.SCP268,
        ItemType.SCP330,
        ItemType.SCP1853,
        ItemType.SCP1576,
        ItemType.SCP2176,
        ItemType.AntiSCP207
    };
    private List<int> customItemIDs4837 = new List<int>
    {
        20, 
        21, 
        22, 
        23,
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        88,
        89,
        90,
        1112,
        1113
    };



    public void SubEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned += OnSchematicSpawned;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
    }

    public void UnsEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
    }

    private void OnRoundStarted()
    {
        _Cooldown = false;
        _hasPlayerPickedUp = false;
        string RoomTradeName = "Scp4837Room";
        Vector3 SCP4837TradeRoomPos = new Vector3(SCP4837.Position.x, SCP4837.Position.y + 20, SCP4837.Position.z);
        Vector3 SCP4837TradeRoomScale = new Vector3(1, 1, 1);
        SCP4837TradeRoom = ObjectSpawner.SpawnSchematic(RoomTradeName, SCP4837TradeRoomPos, SCP4837.Rotation,
            SCP4837TradeRoomScale, MapUtils.GetSchematicDataByName(RoomTradeName), true);
    }

 private void OnSchematicSpawned(SchematicSpawnedEventArgs ev)
{
    if (ev.Schematic == null)
    {
        Log.Error("Schematic is null in OnSchematicSpawned!");
        return;
    }
    if (ev.Schematic.Name.Contains("SCP4837"))
    {
        Log.Info("SCP4837 schematic spawned.");
        var childObjects = ev.Schematic.gameObject.GetComponentsInChildren<Transform>();
        
        SCP4837 = ev.Schematic;
        Log.Info($"Assigned SCP-4837 to {SCP4837.Name}");
    }

    if (ev.Schematic.Name.Contains("Scp4837Room"))
    {
        Transform[] allChildren = ev.Schematic.gameObject.GetComponentsInChildren<Transform>();
        Log.Info("Trade Room spawned!");
        foreach (Transform childTransform in allChildren)
        {
            if (childTransform == null)
                continue;

            if (childTransform.name.Contains("SCP4837Spawn"))
            {
                Scp4837PickupPositionScheme = childTransform;
            }
            else if (childTransform.name.Contains("Scp4837PlayerPosition"))
            {
                SCP4837TradeRoomPlayerPos = childTransform.transform.position;
            }
            else if (childTransform.name.Contains("Scp4837PickupPosition_1"))
            {
                Scp4837PickupPosition_1 = childTransform;
            }
            else if (childTransform.name.Contains("Scp4837PickupPosition_2"))
            {
                Scp4837PickupPosition_2 = childTransform;
            }
            else if (childTransform.name.Contains("Scp4837PickupPosition_3"))
            {
                Scp4837PickupPosition_3 = childTransform;
            }
            else if (childTransform.name.Contains("Scp4837LightSource"))
            {
                SCP4837Light = childTransform;
            }
        }
    }
}
 
public void Trade4837(Player player)
{
    if (player == null)
    {
        Log.Error("Player is null in Trade4837!");
        return;
    }

    if (SCP4837TradeRoomPlayerPos == Vector3.zero)
    {
        Log.Error("SCP4837TradeRoomPlayerPos is not initialized!");
        return;
    }

    Timing.CallDelayed(0.1f, () => { player.Position = SCP4837TradeRoomPlayerPos; });
    player.EnableEffect(EffectType.Flashed, 1, 5);
    SCP4837TradeRoomPlayerPosOld = player.Position;
    Timing.RunCoroutine(StartSCP4837Trade(player));
    _Cooldown = true;

    Scp4837PickupPositionSchemeOld = SCP4837.Position;
    Scp4837PickupRotationSchemeOld = SCP4837.Rotation;

    if (Scp4837PickupPositionScheme != null)
    {
        SCP4837.Position = Scp4837PickupPositionScheme.transform.position;
        SCP4837.Rotation = Scp4837PickupPositionScheme.transform.rotation;
    }
    else
    {
        Log.Error("Scp4837PickupPositionScheme is null in Trade4837!");
    }

    CreateRandomItems(Scp4837PickupPosition_1, Scp4837PickupPosition_2, Scp4837PickupPosition_3, SCP4837TradeItems, customItemIDs4837);
    Timing.RunCoroutine(ColorRandom(player));
}

private void CreateRandomItems(Transform Item1, Transform Item2, Transform Item3, List<ItemType> ItemTypes, List<int> CustomItemTypes)
{
    if (Item1 == null || Item2 == null || Item3 == null)
    {
        Log.Error("One or more item transforms are null in CreateRandomItems!");
        return;
    }

    if ((ItemTypes == null || ItemTypes.Count == 0) && (CustomItemTypes == null || CustomItemTypes.Count == 0))
    {
        Log.Error("Both item type lists are null or empty in CreateRandomItems!");
        return;
    }

    try
    {
        void SpawnRandomItem(Transform transform, int index)
        {
            Pickup spawnedPickup = null;
            bool isCustomItem = UnityEngine.Random.Range(0, 2) == 0;

            if (isCustomItem && CustomItemTypes.Count > 0)
            {
                var customItemId = CustomItemTypes.GetRandomValue();
                Log.Info($"Attempting to spawn custom item with ID {customItemId} at position {transform.position}");
                if (CustomItem.TrySpawn((uint)customItemId, transform.position, out spawnedPickup))
                {
                    spawnedPickup!.Rigidbody.useGravity = false;
                    spawnedPickup!.Rigidbody.isKinematic = true;
                    spawnedPickup!.Rotation = transform.GetChild(0).rotation;
                    
                    // Add to Custom Items list
                    Scp4837Pickups.Add(spawnedPickup);

                    Log.Info($"Successfully spawned custom item with ID {customItemId} at {spawnedPickup.Position} | {spawnedPickup.Rotation} | {transform.GetChild(0).rotation}");
                }
                else
                {
                    Log.Warn($"Failed to spawn custom item with ID {customItemId}");
                }
            }
            else if (ItemTypes.Count > 0)
            {
                var itemType = ItemTypes.GetRandomValue();
                Log.Info($"Spawning regular item of type {itemType} at position {transform.position}");
                spawnedPickup = Pickup.CreateAndSpawn(itemType, transform.position);
                if (spawnedPickup != null)
                {
                    spawnedPickup.Rigidbody.useGravity = false; 
                    spawnedPickup.Rigidbody.isKinematic = true;
                    spawnedPickup.Rotation = transform.GetChild(0).rotation;
                    
                    Scp4837Pickups.Add(spawnedPickup);
                }
            }
            else
            {
                Log.Warn($"No items available to spawn at transform {index}.");
            }
        }
        
        SpawnRandomItem(Item1, 1);
        SpawnRandomItem(Item2, 2);
        SpawnRandomItem(Item3, 3);
    }
    catch (Exception ex)
    {
        Log.Error($"Error while creating random items: {ex}");
    }
}

private void OnPickingUpItem(PickingUpItemEventArgs ev)
{
    if (ev.Pickup == null || ev.Pickup.Base == null)
        return;
    
    foreach (var pickup in Scp4837Pickups)
    {
        if (pickup != null && pickup.Base != null && ev.Pickup.Base.name.Contains(pickup.Base.name))
        {
            _hasPlayerPickedUp = true;
            
            foreach (var otherPickup in Scp4837Pickups)
            {
                if (otherPickup != pickup)
                {
                    DestroyPickup(otherPickup);
                }
            }

            Scp4837Pickups.Clear();
            Scp4837Pickups.Add(pickup);

            Timing.CallDelayed(1.2f, () => { _hasPlayerPickedUp = false; });
            return;
        }
    }
}

private void DestroyPickup(Pickup pickup)
{
    if (pickup != null)
    {
        pickup.Destroy();
    }
}


    private IEnumerator<float> ColorRandom(Player player)
    {
        SSTwoButtonsSetting ShowRainBowColor = player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(416);
        if (!ShowRainBowColor.SyncIsB)
        {
            float hue = 0f;
            const float hueIncrement = 0.05f;
            const float updateInterval = 0.1f;
            Light light = Light.Create(SCP4837Light.position);
            light.Intensity = 15f;
            light.Range = 15f;
            while (Scp4837TradeDur > 0)
            {
                if (_hasPlayerPickedUp)
                {
                    light.Destroy();
                    yield break;
                }

                light.Color = Color.HSVToRGB(hue, 1f, 1f);
                hue += hueIncrement;

                if (hue > 1f)
                {
                    hue = 0f;
                }
                
                yield return Timing.WaitForSeconds(updateInterval);
            }
            light.Destroy();
            yield break;
        }
        if (ShowRainBowColor.SyncIsB)
        {
            Light light = Light.Create(SCP4837Light.position);
            light.Intensity = 15f;
            light.Range = 15f;
            while (Scp4837TradeDur > 0)
            {
                if (_hasPlayerPickedUp)
                {
                    light.Destroy();
                    yield break;
                }

                light.Color = Plugin.Singleton.PlayerColorManager.GetPlayerColor(player);
                Log.Info(light.Color);
                Log.Info(Plugin.Singleton.PlayerColorManager.GetPlayerColor(player));
                
                yield return Timing.WaitForSeconds(1);
            }
            light.Destroy();
            yield break;
        }
        yield break;
    }
    

    
    private List<string> RandomSounds = new List<string>
    {
        "Horror1.ogg",
        "Horror3.ogg",
        "Horror9.ogg",
        "Horror10.ogg",
        "Horror14.ogg",
        "Horror7.ogg"
    };
    private IEnumerator<float> StartSCP4837Trade(Player player)
    {
        SSTwoButtonsSetting PlayCustomSounds = player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(413);
        SSTwoButtonsSetting PlayCustomSoundsHorror = player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(414);
        Timing.RunCoroutine(Timer4837(player));
        player.EnableEffect(EffectType.Ensnared, 1, Scp4837TradeDur);
        player.EnableEffect(EffectType.AmnesiaVision, 1, 30);
        player.EnableEffect(EffectType.Burned, 1, 30);
        if (PlayCustomSoundsHorror.SyncIsA)
        {
            Timing.CallDelayed(0.11f, () => { PlayRandomSound(player, false); });
            
        }
        if (PlayCustomSounds.SyncIsA)
        {
            Timing.CallDelayed(0.11f, () => { player.Position.SpecialPosExtra("4837Pocket.ogg", 15, 1, 30); });   
        }

        while (Scp4837TradeDur > 0)
        {
            if (_hasPlayerPickedUp)
            {
                player.DisableEffect(EffectType.Ensnared);
                player.DisableEffect(EffectType.Burned);
                player.DisableEffect(EffectType.AmnesiaVision);
                player.EnableEffect(EffectType.Flashed, 1, 5);
                player.EnableEffect(EffectType.Ensnared, 1, 5);
                player.Position = SCP4837TradeRoomPlayerPosOld;
                Scp4837Pickups.Clear();
                if (PlayCustomSoundsHorror.SyncIsA)
                {
                    PlayRandomSound(player, true);
                }

                Timing.CallDelayed(45 + Scp4837TradeDur,
                    () =>
                    {
                        Log.Info("SCP-4837 wieder aktiv!");
                        SCP4837.Position = Scp4837PickupPositionSchemeOld;
                        SCP4837.Rotation = Scp4837PickupRotationSchemeOld;
                    });
                _Cooldown = false;
                yield break;
            }
            
            yield return Timing.WaitForSeconds(0.1f);
        }
        player.DisableEffect(EffectType.Ensnared);
        player.DisableEffect(EffectType.Burned);
        player.DisableEffect(EffectType.AmnesiaVision);
        player.EnableEffect(EffectType.Flashed, 1, 5);
        player.EnableEffect(EffectType.Ensnared, 1, 5);
        Scp4837Pickups.Clear();
        DestroyAllPickups();
        player.Position = SCP4837TradeRoomPlayerPosOld;
        if (PlayCustomSoundsHorror.SyncIsA)
        {
            PlayRandomSound(player, true);
        }
        

        Timing.CallDelayed(45f,
            () =>
            {
                Log.Info("SCP-4837 wieder aktiv!");
                SCP4837.Position = Scp4837PickupPositionSchemeOld;
                SCP4837.Rotation = Scp4837PickupRotationSchemeOld;

                _Cooldown = false;
            });

        yield break;
    }

    private IEnumerator<float> Timer4837(Player player)
    {
        while (Scp4837TradeDur > 0)
        {
            if (_hasPlayerPickedUp)
            {
                Scp4837TradeDur = 30f;
                yield break;
            }
            player.ShowMeowHintDur($"Du hast noch <b>{Scp4837TradeDur} Sekunden</b>, um mit <b><color=#757935>SCP-4837</b></color> zu traden!", 1);

            Scp4837TradeDur--;
            yield return Timing.WaitForSeconds(1f);
        }
        player.ShowMeowHint("Die Zeit ist abgelaufen! Du wurdest zurück <b>Teleportiert</b>.");
        Scp4837TradeDur = 30f;
        yield break;
    }


    private void DestroyAllPickups()
    {
        var pickupsToRemove = new List<Pickup>(Scp4837Pickups);
        foreach (var pickup in pickupsToRemove)
        {
            pickup.Destroy();
            Scp4837Pickups.Remove(pickup);
        }
    }
    private void PlayRandomSound(Player player, bool leaving)
    {
        string randomSound = RandomSounds.GetRandomValue();
        
        player.Position.SpecialPosExtra(randomSound, 2, 2,
            leaving ? 15 : 16); 
    }
}