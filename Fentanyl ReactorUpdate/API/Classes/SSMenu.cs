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
        RegisterSSButton();
    }

    public void UnSubEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.Verified -= OnPlayerVerified;
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnButtonTriggered;
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
        
        DropdownSetting startreactor = new(5, "label", new[] { "Off", "Startup", "Maintenance" }, 5, 5, "hintDescription");

    IEnumerable<SettingBase> settings = new SettingBase[]
        {
            admin,
            testButton,
            player,
            fuelreactor
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
            if (!player.CheckPermission(Plugin.Singleton.Translation.TeleportFentanylPremission))
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.TeleportFentanylNoPrem);
                return;
            }
            player.Position = _roomPos;
        }
        if (settingBase is SSButton fuelButton && fuelButton.SettingId == Plugin.Singleton.Config.ServerSpecificSettingIdFuel)
        {
            if (Vector3.Distance(player.Position, _roomPos) <= 12 && player.Position.y >= Plugin.Singleton.Reactor.RoomScheme.Position.y - 10)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.FentanylReactorSSFuel);
                return;
            }

            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.FuelCommandName} {player.UserId}");
        }
    }
    
    private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
    {
    }
}

