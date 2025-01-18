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
using Fentanyl_ReactorUpdate.API.CustomItems;
using Fentanyl_ReactorUpdate.Configs;
using MEC;
using Mirror;
using UnityEngine;
using Random = System.Random;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.API.SCP1356;
using Fentanyl_ReactorUpdate.API.SCP1356.Events;
using Fentanyl_ReactorUpdate.API.SCP4837;
using UnityEngine.PlayerLoop;
using UserSettings.ServerSpecific;

namespace Fentanyl_ReactorUpdate;

public class Plugin : Plugin<Configs.Config, Configs.Translation>
{
    public ForceReactorMeltdownCommand MeltdownCommandInstance { get; private set; }
    public override string Name => "Fentanyl Reactor";
    public override string Author => "SCP: Secret Fentanyl Server Team";
    public override Version Version => new Version(1, 5, 3);
    public override Version RequiredExiledVersion => new Version(9, 2, 1);
    public static Plugin Singleton = new Plugin();
    public static readonly Random Random = new Random();
    public Reactor Reactor { get; private set; }
    public DevNuke DevNuke { get; private set; }
    public Elevator Elevator { get; private set; }
    public KillAreaCommand KillAreaCommand { get; private set; }
    public RadiationDamage RadiationDamage { get; private set; }
    public FentGenerator FentGenerator { get; private set; }
    public RoundSummaryText RoundSummaryText { get; private set; }
    public SCP4837InteractionMenu SCP4837InteractionMenu { get; private set; }
    public AudioPlayerRandom AudioPlayerRandom { get; private set; }
    public MeltdownAutoStart MeltdownAutoStart { get; private set; }
    public Contain Contain { get; private set; }
    public Breach Breach { get; private set; }
    public Main4837 Main4837 { get; private set; }
    public CustomItemLight CustomItemLight { get; private set; }
    public bool SCP1356Breach { get; set; }
    public Brot brot { get; set;  }
    
    public MainDuck MainDuck { get; private set; }
        
    public override void OnEnabled()
    {
        brot = new Brot();
        MeltdownCommandInstance = new ForceReactorMeltdownCommand();
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorTest.ogg"), "Fentanyl Reactor");
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorMeltdown.ogg"), "Fentanyl Reactor Meltdown");
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "DemonCore.ogg"), "Fentanyl Reactor Demon Core");
        Singleton = this;
        Elevator = new Elevator();
        Elevator.SubEvents();
        AudioPlayerRandom = new AudioPlayerRandom();
        RadiationDamage = new RadiationDamage();
        DevNuke = new DevNuke();
        FentGenerator = new FentGenerator();
        FentGenerator.SubEvents();
        DevNuke.SubEvents();
        Main4837 = new Main4837();
        Main4837.SubEvents();
        CustomItemLight = new CustomItemLight();
        CustomItemLight.SubscribeEvents();
        Contain = new Contain();
        Contain.SubEvents();
        KillAreaCommand = new KillAreaCommand();
        KillAreaCommand.SubEvents();
        MainDuck = new MainDuck();
        MainDuck.SubEvents();
        CustomItem.RegisterItems();
        RoundSummaryText = new RoundSummaryText();
        RoundSummaryText.SubEvents();
        Reactor = new Reactor();
        Breach = new Breach();
        Breach.SubEvents();
        if (Plugin.Singleton.Config.Update)
        {
            UpdateChecker.RegisterEvents();
            UpdateSchematicChecker.RegisterEvents();
            UpdateOggReactor.RegisterEvents();
            UpdateOggMeltdown.RegisterEvents();
            UpdateOggDemonCore.RegisterEvents();
            UpdateSchematicDemonCoreChecker.RegisterEvents();
        }
        SCP4837InteractionMenu = new SCP4837InteractionMenu();
        ServerSpecificSyncer.Features.Menu.RegisterAll();
        MeltdownAutoStart = new MeltdownAutoStart();
        base.OnEnabled();
    }

        
    public override void OnDisabled()
    {
        brot = null;
        DevNuke.UnsubEvents();
        DevNuke = null;
        Main4837.UnsEvents();
        Main4837 = null;
        MainDuck.UnsubEvents();
        MainDuck = null;
        Elevator.UnsubEvents();
        Elevator = null;
        Breach.UnsubEvents();
        Breach = null;
        Contain.UnsubEvents();
        Contain = null;
        KillAreaCommand.UnSubEvents();
        CustomItemLight.UnsubscribeEvents();
        CustomItemLight = null;
        base.OnDisabled();
        CustomRole.UnregisterRoles();
        Reactor.Destroy();
        RoundSummaryText.UnsubEvents();
        RoundSummaryText = null;
        MeltdownAutoStart.Destroy();
        FentGenerator.UnsubEvents();
        FentGenerator = null;
        RadiationDamage = null;
        AudioPlayerRandom = null;
        Reactor = null;
        KillAreaCommand = null;
        if (Plugin.Singleton.Config.Update)
        {
            UpdateChecker.UnRegisterEvents();
            UpdateSchematicChecker.UnRegisterEvents();
            UpdateOggReactor.UnRegisterEvents();
            UpdateOggMeltdown.UnRegisterEvents();
            UpdateOggDemonCore.UnRegisterEvents();
            UpdateSchematicDemonCoreChecker.UnRegisterEvents();
        }
        ServerSpecificSyncer.Features.Menu.UnregisterAll();
        SCP4837InteractionMenu = null;
        MeltdownAutoStart.UnSubEvents();
        CustomItem.UnregisterItems();
        Singleton = null;
        base.OnDisabled();
    }
}