using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Fentanyl_ReactorUpdate.API;
using Fentanyl_ReactorUpdate.API.Classes;
using Fentanyl_ReactorUpdate.API.Commands;
using Fentanyl_ReactorUpdate.Configs;
using MEC;
using Mirror;
using UnityEngine;
using Random = System.Random;
using Fentanyl_ReactorUpdate.API.Commands;
using Fentanyl_ReactorUpdate.API.Extensions;
using UnityEngine.PlayerLoop;
using UserSettings.ServerSpecific;

namespace Fentanyl_ReactorUpdate;

public class Plugin : Plugin<Configs.Config, Configs.Translation>
{
    public ForceReactorMeltdownCommand MeltdownCommandInstance { get; private set; }
    public override string Name => "Fentanyl Reactor";
    public override string Author => "SCP: Secret Fentanyl Server Team";
    public override Version Version => new Version(1, 4, 9);
    public override Version RequiredExiledVersion => new Version(9, 1, 0);
    public static Plugin Singleton = new Plugin();
    public static readonly Random Random = new Random();
    public Reactor Reactor { get; private set; }
    
    public SSMenu SsMenu { get; private set; }
    public MeltdownAutoStart MeltdownAutoStart { get; private set; }
        
    public override void OnEnabled()
    {
        MeltdownCommandInstance = new ForceReactorMeltdownCommand();
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorTest.ogg"), "Fentanyl Reactor");
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorMeltdown.ogg"), "Fentanyl Reactor Meltdown");
        Singleton = this;
        CustomItem.RegisterItems();
        Reactor = new Reactor();
        UpdateChecker.RegisterEvents();
        UpdateSchematicChecker.RegisterEvents();
        UpdateOggReactor.RegisterEvents();
        UpdateOggMeltdown.RegisterEvents();
        SsMenu = new SSMenu();
        MeltdownAutoStart = new MeltdownAutoStart();
        base.OnEnabled();
    }

        
    public override void OnDisabled()
    {
        CustomRole.UnregisterRoles();
        Reactor.Destroy();
        MeltdownAutoStart.Destroy();
        Reactor = null;
        UpdateChecker.UnRegisterEvents();
        UpdateSchematicChecker.UnRegisterEvents();
        UpdateOggReactor.UnRegisterEvents();
        UpdateOggMeltdown.UnRegisterEvents();
        SsMenu.Destroy();
        MeltdownAutoStart.UnSubEvents();
        CustomItem.UnregisterItems();
        Singleton = null;
        base.OnDisabled();
    }
}