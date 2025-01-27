using System;
using System.IO;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Fentanyl_ReactorUpdate.API;
using Fentanyl_ReactorUpdate.API.Classes;
using Fentanyl_ReactorUpdate.API.Commands;
using Fentanyl_ReactorUpdate.API.CustomItems;
using Fentanyl_ReactorUpdate.API.Database;
using Random = System.Random;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.API.SCP1356;
using Fentanyl_ReactorUpdate.API.SCP1356.Events;
using Fentanyl_ReactorUpdate.API.SCP4837;
using Fentanyl_ReactorUpdate.API.SCPBuffs.SCP049;


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
    public CustomItemSchematic CustomItemSchematic { get; private set; }
    public KillAreaCommand KillAreaCommand { get; private set; }
    public RadiationDamage RadiationDamage { get; private set; }
    public FentGenerator FentGenerator { get; private set; }
    public RoundSummaryText RoundSummaryText { get; private set; }
    public WebSocketServer WebSocketServer { get; private set; }
    public MoneyEvents MoneyEvents { get; private set; }
    public SCP4837InteractionMenu SCP4837InteractionMenu { get; private set; }
    public AudioPlayerRandom AudioPlayerRandom { get; private set; }
    public MeltdownAutoStart MeltdownAutoStart { get; private set; }
    public AntiCamp AntiCamp { get; private set; }
    public Contain Contain { get; private set; }
    public Breach Breach { get; private set; }
    public Main4837 Main4837 { get; private set; }
    public CustomItemLight CustomItemLight { get; private set; }
    public PlayerColorManager PlayerColorManager { get; private set; }
    public bool SCP1356Breach { get; set; }
    public Brot brot { get; set;  }
    public Enmm Enmm { get; set; }
    public SCP049Buff SCP049Buff { get; set; }
    public CraftingSystem CraftingSystem { get; set; }
    public AdminMenu AdminMenu { get; set; }
    
    public MainDuck MainDuck { get; private set; }
        
    public override void OnEnabled()
    {
        //LiteSQL.Connect();
       // AdminMenu = new AdminMenu();
       AntiCamp = new AntiCamp();
       AntiCamp.SubEvents();
        Enmm = new Enmm();
        SCP049Buff = new SCP049Buff();
        SCP049Buff.SubEvents();
        if (Singleton.Config.CraftingSystem)
        {
            CraftingSystem = new CraftingSystem();
            CraftingSystem.Initialize();
        }
        WebSocketServer = new WebSocketServer();
        WebSocketServer.SubEvents();
        MoneyEvents = new MoneyEvents();
        MoneyEvents.SubscribeEvents();
        brot = new Brot();
        MeltdownCommandInstance = new ForceReactorMeltdownCommand();
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorTest.ogg"), "Fentanyl Reactor");
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorMeltdown.ogg"), "Fentanyl Reactor Meltdown");
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "DemonCore.ogg"), "Fentanyl Reactor Demon Core");
        Singleton = this;
        Elevator = new Elevator();
        CustomItemSchematic = new CustomItemSchematic();
        CustomItemSchematic.SubscribeEvents();
        Elevator.SubEvents();
        AudioPlayerRandom = new AudioPlayerRandom();
        DevNuke = new DevNuke();
        FentGenerator = new FentGenerator();
        PlayerColorManager = new PlayerColorManager();
        FentGenerator.SubEvents();
        DevNuke.SubEvents();
        CustomItemLight = new CustomItemLight();
        CustomItemLight.SubscribeEvents();
        CustomItem.RegisterItems();
        RoundSummaryText = new RoundSummaryText();
        RoundSummaryText.SubEvents();
        Reactor = new Reactor();
        if (Singleton.Config.DemonCore)
        {
            KillAreaCommand = new KillAreaCommand();
            KillAreaCommand.SubEvents();
        }
        if (Plugin.Singleton.Config.SCP1356)
        {
            RadiationDamage = new RadiationDamage();
            MainDuck = new MainDuck();
            MainDuck.SubEvents();
            Breach = new Breach();
            Breach.SubEvents();
            Contain = new Contain();
            Contain.SubEvents();
        }
        if (Plugin.Singleton.Config.SCP4837)
        {
            Main4837 = new Main4837();
            Main4837.SubEvents();
        }
        if (Plugin.Singleton.Config.Update)
        {
            UpdateChecker.RegisterEvents();
        }
        SCP4837InteractionMenu = new SCP4837InteractionMenu();
        SSMenuSystem.Features.Menu.RegisterAll();
        MeltdownAutoStart = new MeltdownAutoStart();
        base.OnEnabled();
    }
        
    public override void OnDisabled()
    {
       // LiteSQL.Disconnect();
        brot = null;
        DevNuke.UnsubEvents();
        DevNuke = null;
        Elevator.UnsubEvents();
        Elevator = null;
        AntiCamp.UnsubEvents();
        AntiCamp = null;
        PlayerColorManager = null;
        if (Singleton.Config.CraftingSystem)
        {
            CraftingSystem.Uninitialize();
            CraftingSystem = null;
        }
        Contain.UnsubEvents();
        Enmm = null;
        CustomItemSchematic.UnsubscribeEvents();
        CustomItemSchematic = null;
        SCP049Buff.UnSubEvents();
        SCP049Buff = null;
        CustomItemLight.UnsubscribeEvents();
        CustomItemLight = null;
        MoneyEvents.UnsubscribeEvents();
        MoneyEvents = null;
        //AdminMenu = null;
        base.OnDisabled();
        CustomRole.UnregisterRoles();
        Reactor.Destroy();
        RoundSummaryText.UnsubEvents();
        RoundSummaryText = null;
        MeltdownAutoStart.Destroy();
        FentGenerator.UnsubEvents();
        FentGenerator = null;
        AudioPlayerRandom = null;
        Reactor = null;
        WebSocketServer.UnSubEvents();
        WebSocketServer = null;
        if (Plugin.Singleton.Config.DemonCore)
        {
            KillAreaCommand.UnSubEvents();   
            KillAreaCommand = null;
        }
        if (Plugin.Singleton.Config.SCP1356)
        {
            MainDuck.UnsubEvents();
            MainDuck = null;
            Breach.UnsubEvents();
            Breach = null;
            Contain.UnsubEvents();
            Contain = null;
            RadiationDamage = null;
        }
        if (Plugin.Singleton.Config.SCP4837)
        {
            Main4837.UnsEvents();
            Main4837 = null;
        }
        if (Plugin.Singleton.Config.Update)
        {
            UpdateChecker.UnRegisterEvents();
        }
        SSMenuSystem.Features.Menu.Unregister(SCP4837InteractionMenu);
        SCP4837InteractionMenu = null;
        MeltdownAutoStart.UnSubEvents();
        CustomItem.UnregisterItems();
        Singleton = null;
        base.OnDisabled();
    }
}