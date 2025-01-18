using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.Commands.Hub;
using Exiled.Events.EventArgs.Player;
using Fentanyl_ReactorUpdate.API.Classes;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.API.SCP4837;
using Fentanyl_ReactorUpdate.Configs;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.Events.EventArgs;
using MEC;
using PlayerRoles;
using ServerSpecificSyncer.Features;
using UnityEngine;
using UserSettings.ServerSpecific;
using Light = Exiled.API.Features.Toys.Light;

namespace Fentanyl_ReactorUpdate.API.CustomItems;

[CustomItem(ItemType.KeycardJanitor)]
public class Brot : CustomItem
{
    private static readonly Config Config = Plugin.Singleton.Config;
    private static readonly Translation Translation = Plugin.Singleton.Translation;
    public override string Name { get; set; } = "Brot";
    public override string Description { get; set; } = "Leckeres Brot, um mit SCP-4837 zu handeln!";
    public override float Weight { get; set; } = Config.T1Weight;
    public override uint Id { get; set; } = 1489;
    public override SpawnProperties SpawnProperties { get; set; }
    private Dictionary<Pickup, SchematicObject> ActiveBreads { get; set; } = new();
    private Dictionary<Pickup, Light> ActiveLights { get; set; } = new();

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
    }

    private void OnRoundStarted()
    {
        Timing.RunCoroutine(CheckSCP4837Range());
    }

    private void OnDroppedItem(DroppedItemEventArgs ev)
    {
        if (ev.Pickup == null || !Check(ev.Pickup))
            return;

        Log.Info($"Player {ev.Player.Nickname} hat das Brot fallen gelassen bei {ev.Pickup.Position}.");
        Timing.RunCoroutine(SpawnBreadSchematicAndLight(ev.Pickup));
    }


    private void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if (ev.Pickup == null || !Check(ev.Pickup))
            return;

        if (!ActiveBreads.TryGetValue(ev.Pickup, out var breadSchematic))
            return;

        Log.Info($"Player {ev.Player.Nickname} hat das Brot aufgehoben bei {ev.Pickup.Position}.");
        breadSchematic.Destroy();
        ActiveBreads.Remove(ev.Pickup);

        if (ActiveLights.TryGetValue(ev.Pickup, out var light))
        {
            light.Destroy();
            ActiveLights.Remove(ev.Pickup);
        }
    }

    private IEnumerator<float> SpawnBreadSchematicAndLight(Pickup pickup)
    {
        if (pickup == null)
            yield break;

        const string SchemeName = "Bread4837";
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
            $"Spawned Bread schematic at {pickup.Position} with rotation {breadSchematic.transform.rotation.eulerAngles}.");
        ActiveBreads[pickup] = breadSchematic;

        var light = Exiled.API.Features.Toys.Light.Create(
            pickup.Position);
        light.Color = new Color(238f / 255f, 192f / 255f, 123f / 255f);
        light.Position = new Vector3(pickup.Position.x, pickup.Position.y + 0.2f, pickup.Position.z);
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
    

    private IEnumerator<float> CheckSCP4837Range()
    {
        List<string> noBreadAudios = new()
        {
            "scp4837_bread_1.ogg",
            "scp4837_bread_2.ogg",
        };

        List<string> hasBreadAudios = new()
        {
            "2384_Ci.ogg",
            "2384_ClassD.ogg",
            "2384_Guard.ogg",
            "2384_Mtf.ogg",
            "2384_ScD.ogg"
        };

        Dictionary<Player, bool> playerInRangeStatus = new();

        while (Round.IsStarted && !Round.IsEnded && !Warhead.IsDetonated)
        {
            foreach (var player in Player.List)
            {
                bool isInRange = Vector3.Distance(player.Position, Plugin.Singleton.Main4837.SCP4837.Position) < 2.5f;

                if (isInRange && (!playerInRangeStatus.ContainsKey(player) || !playerInRangeStatus[player]))
                {
                    playerInRangeStatus[player] = true;

                    if (Plugin.Singleton.Main4837._Cooldown)
                        continue;
                    
                    SSTwoButtonsSetting ShowTutHint = player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(412);
                    SSTwoButtonsSetting PlayCustomSounds = player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(413);
                    
                    if (ShowTutHint.SyncIsA)
                    {
                        player.ShowMeowHint("<color=yellow>🔒</color> Um mit <b><color=#757935>SCP-4837</b></color> zu interagieren, musst du einen Keybind festlegen! \n ESC => Settings / Einstellungen => Server-Specific / Server-Spezifisch \n Dort kannst du auch diesen Hint Deaktivieren!");
                        if (PlayCustomSounds.SyncIsA)
                        {
                            string randomAudio = noBreadAudios.GetRandomValue();
                            Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos(randomAudio, 10, 7);
                        }
                        continue;
                    }
                    
                    if (player.Role.Team == Team.SCPs)
                        {
                            player.ShowMeowHint("<color=yellow>🔒</color> SCPs können nicht mit <b><color=#757935>SCP-4837</b></color> interagieren.");
                            if (PlayCustomSounds.SyncIsA)
                            {
                                Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos("2384_Scp.ogg", 10, 15);
                            }
                            continue;
                        }
                    
                        if (player.Role.Type == RoleTypeId.Tutorial)
                        {
                            player.ShowMeowHint($"Willkommen, {player.Nickname}! \n" +
                                                "Als Mitglied der <b><color=yellow>D.U.C.K.</color></b> kannst du kostenlos mit <b><color=#757935>SCP-4837</b></color> handeln. \n" +
                                                "📜 Nähere dich <b><color=#757935>SCP-4837</b></color>, um mehr zu erfahren.");
                            if (PlayCustomSounds.SyncIsA)
                            {
                                Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos("2384_tut.ogg", 10, 7);
                            }
                            continue;
                        }

                        if (player.CurrentItem is null || !Check(player.CurrentItem))
                        {
                            player.ShowMeowHint(
                                $"⚠️ Du brauchst Brot, um mit <b><color=#757935>SCP-4837</b></color> zu handeln! \n" +
                                $"🔎 Suche in der Facility nach Brot und kehre zurück, um zu handeln & drücke <b>''</b>, während du auf <b><color=#757935>SCP-4837</b></color> schaust.");
                            if (PlayCustomSounds.SyncIsA)
                            {
                                string randomAudio = noBreadAudios.GetRandomValue();
                                Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos(randomAudio, 10, 7);
                            }
                            continue;
                        }

                        if (Check(player.CurrentItem))
                        {
                            player.ShowMeowHint($"✨ Willkommen, {player.Nickname}! \n" +
                                                "Du hast <b>Brot</b> dabei. \n" +
                                                $"Drücke <b>''</b>, um mit <b><color=#757935>SCP-4837</b></color> zu handeln und besondere Vorteile zu erhalten!");
                            if (PlayCustomSounds.SyncIsA)
                            {
                                string randomAudio = hasBreadAudios.GetRandomValue();
                                Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos(randomAudio, 10, 7);
                            }
                        }
                    }
                    else if (!isInRange && playerInRangeStatus.ContainsKey(player) && playerInRangeStatus[player])
                    {
                        playerInRangeStatus[player] = false;
                    }
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }