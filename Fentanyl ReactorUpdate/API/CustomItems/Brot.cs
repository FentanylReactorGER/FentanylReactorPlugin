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
using Exiled.Events.EventArgs.Map;
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
using SSMenuSystem.Features;
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
        Exiled.Events.Handlers.Player.TogglingNoClip += OnStandartButton;
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.TogglingNoClip -= OnStandartButton;
    }

    private void OnRoundStarted()
    {
        Timing.RunCoroutine(CheckSCP4837Range());
    }
    
    private void OnStandartButton(TogglingNoClipEventArgs ev)
    {
        SSTwoButtonsSetting UseNoclipKey = ev.Player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(417);
        if (UseNoclipKey.SyncIsA)
        {
            Plugin.Singleton.SCP4837InteractionMenu.StartTrading4837(ev.Player);
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
                    
                    if (Plugin.Singleton.Main4837.PlayerTradeCounts.ContainsKey(player) && Plugin.Singleton.Main4837.PlayerTradeCounts[player] >= 3)
                    {
                        player.ShowMeowHint("<color=yellow>⚠️</color> Du hast bereits dreimal mit <b><color=#757935>SCP-4837</b></color> gehandelt.\n Du kannst nächste Runde erneut handeln!");
                        Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos("2384_ScD.ogg", 10, 9);
                        continue;
                    }
                    
                    if (ShowTutHint.SyncIsA)
                    {
                        player.ShowMeowHint("<color=yellow>🔒</color> Um mit <b><color=#757935>SCP-4837</b></color> zu interagieren, musst du einen Keybind festlegen! \n ESC => Settings / Einstellungen => Server-Specific / Server-Spezifisch \n Dort kannst du auch diesen Hint Deaktivieren! \n Standart: ALT");
                        if (PlayCustomSounds.SyncIsA)
                        {
                            string randomAudio = noBreadAudios.GetRandomValue();
                            Plugin.Singleton.Main4837.SCP4837.Position.SpecialPos(randomAudio, 10, 9);
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
                                $"🔎 Suche in der Facility nach Brot und kehre zurück, um zu handeln & drücke Standart: <b>'ALT'</b>, während du auf <b><color=#757935>SCP-4837</b></color> schaust.");
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
                                                $"Drücke Standart: <b>'ALT'</b>, um mit <b><color=#757935>SCP-4837</b></color> zu handeln und besondere Vorteile zu erhalten!");
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