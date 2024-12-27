using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using Fentanyl_ReactorUpdate.API.Classes;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Fentanyl_ReactorUpdate.API.Classes;

public class SSMenu
{
    private static float XPos { get; set; }
    private static float YPos { get; set; }
    private static float ZPos { get; set; }

    public Vector3 _roomPos = new Vector3(XPos, YPos, ZPos);
    
    public SSMenu()
    {
        SubEvents();
    }

    public void Destroy()
    {
        UnSubEvents();
    }
    public void SubEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.Verified += OnPlayerVerified;
        ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnButtonTriggered;
        ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnDropdownTriggered;
        RegisterSSButton();
    }

    public void UnSubEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.Verified -= OnPlayerVerified;
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnButtonTriggered;
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnDropdownTriggered;
    }

    public void OnPlayerVerified(VerifiedEventArgs ev)
    {
        ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
    }

    public void RegisterSSButton()
    {
        HeaderSetting admin = new("Fentanyl Reaktor Admin");

        ButtonSetting testButton = new(
            Plugin.Singleton.Config.ServerSpecificSettingId,
            Plugin.Singleton.Translation.SSSSLabelTp,
            Plugin.Singleton.Translation.SSSSTpButton,
            Plugin.Singleton.Config.ServerSpecificSettingHoldTime,
            Plugin.Singleton.Translation.SSSSDescTp);
        
        HeaderSetting player = new(Plugin.Singleton.Translation.SSSSheaderPlayer);

        ButtonSetting fuelreactor = new(
            Plugin.Singleton.Config.ServerSpecificSettingIdFuel,
            Plugin.Singleton.Translation.SSSSLabelFuel,
            Plugin.Singleton.Translation.SSSSFuelButton,
            Plugin.Singleton.Config.ServerSpecificSettingHoldTime,
            Plugin.Singleton.Translation.SSSSDescFuel);
        
        DropdownSetting startreactor = new(Plugin.Singleton.Config.ServerSpecificSettingIdStart, Plugin.Singleton.Translation.SSSSStartName, new[] { Plugin.Singleton.Translation.SSSSlStage1, Plugin.Singleton.Translation.SSSSlStage2, Plugin.Singleton.Translation.SSSSlStage3 }, 1, dropdownEntryType: SSDropdownSetting.DropdownEntryType.Regular,  Plugin.Singleton.Translation.SSSSStartDesc);

    IEnumerable<SettingBase> settings = new SettingBase[]
        {
            admin,
            testButton,
            player,
            fuelreactor,
            startreactor
        };
        SettingBase.Register(settings);
        SettingBase.SendToAll();
    }
        
    public void OnRoundStarted()
    {
        XPos = Plugin.Singleton.Reactor.RoomScheme.Position.x;
        YPos = Plugin.Singleton.Reactor.RoomScheme.Position.y + 3;
        ZPos = Plugin.Singleton.Reactor.RoomScheme.Position.z;
        _roomPos = new Vector3(XPos, YPos, ZPos);
        Log.Info("Round Started");
        Log.Info($"{Plugin.Singleton.Reactor.RoomScheme.Position} {_roomPos}");
    }

    private void OnButtonTriggered(ReferenceHub hub, ServerSpecificSettingBase settingBase)
    {
        if (!Player.TryGet(hub, out Player player))
        {
            return;
        }

        if (settingBase is SSButton fentTpButton && fentTpButton.SettingId == Plugin.Singleton.Config.ServerSpecificSettingId)
        {
            if (!Round.IsStarted)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSRoundNotStarted);
                return;
            }
            if (player.IsScp)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSPlayerIsSCP);
                return;
            }
            if (!player.CheckPermission(Plugin.Singleton.Translation.TeleportFentanylPremission))
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.TeleportFentanylNoPrem);
                return;
            }
            player.Position = _roomPos;
        }
        if (settingBase is SSButton fuelButton && fuelButton.SettingId == Plugin.Singleton.Config.ServerSpecificSettingIdFuel)
        {
            if (!Round.IsStarted)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSRoundNotStarted);
                return;
            }

            if (player.IsScp)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSPlayerIsSCP);
                return;
            }
            if (Vector3.Distance(player.Position, _roomPos) > 12)
            {
                if (player.Position.y >= -1015 && player.Position.y <= -1005)
                {
                    player.ShowMeowHint(Plugin.Singleton.Translation.FentanylReactorSSFuel);
                    return;
                }
            }
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.FuelCommandName} {player.Id}");
        }
    }

    private void OnDropdownTriggered(ReferenceHub hub, ServerSpecificSettingBase settingBase)
    {
        if (!Player.TryGet(hub, out Player player))
        {
            return;
        }

        if (settingBase is SSDropdownSetting startreactor && startreactor.SettingId == Plugin.Singleton.Config.ServerSpecificSettingIdStart)
        {
            if (Round.IsStarted)
            {
                if (player.IsScp)
                {
                    player.ShowMeowHint(Plugin.Singleton.Translation.SSSSPlayerIsSCP);
                    return;
                }
                if (Vector3.Distance(player.Position, _roomPos) <= 12)
                {
                        if (startreactor.SyncSelectionText == Plugin.Singleton.Translation.SSSSlStage1)
                        {
                            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {player.Id} 1");
                        }
                        if (startreactor.SyncSelectionText == Plugin.Singleton.Translation.SSSSlStage2)
                        {
                            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {player.Id} 2");
                        }
                        if (startreactor.SyncSelectionText == Plugin.Singleton.Translation.SSSSlStage3)
                        {
                            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {player.Id} 3");
                        }
                }
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSStartNotInReactor);
            }
        }
    }
    
    private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
    {
    }
}

