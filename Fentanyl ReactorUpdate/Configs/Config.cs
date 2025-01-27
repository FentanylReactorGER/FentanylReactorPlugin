using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using PluginAPI.Core;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.Configs;

public class Preset
{
    public string Name { get; set; }
    public ItemType? Item { get; set; }
    public uint? CustomItemId { get; set; }
    public float Price { get; set; }

    public Preset() { }

    public Preset(string name, ItemType item, float price)
    {
        Name = name;
        Item = item;
        CustomItemId = null;
        Price = price;
    }

    public Preset(string name, uint customItemId, float price)
    {
        Name = name;
        Item = null;
        CustomItemId = customItemId;
        Price = price;
    }
}

public class Config : IConfig
{
    [Description("Should the plugin be enabled")]
    public bool IsEnabled { get; set; } = true;

    [Description("Should the plugin display a debug message")]
    public bool Debug { get; set; } = false;

    [Description("Custom Item ID, RGB Color.")]
    public Dictionary<uint, Color> CustomItemLightColors { get; set; } = new()
    {
        { 88, new Color(1f, 0.41f, 0.15f) },
        { 89, new Color(0.3f, 1f, 0.3f) },
        { 90, new Color(0.15f, 0.45f, 1f) },
        { 20, new Color(1f, 1f, 0.15f) },
        { 21, new Color(0.85f, 0.14f, 0.14f) },
        { 22, new Color(0.8f, 0.5f, 0.2f) },
        { 23, new Color(0.6f, 0.1f, 0.5f) },
        { 24, new Color(0.95f, 0.7f, 0.1f) },
        { 25, new Color(0.34f, 0.17f, 0.72f) },
        { 26, new Color(0.14f, 0.8f, 0.72f) },
        { 27, new Color(0.9f, 0.3f, 0.7f) },
        { 28, new Color(0.25f, 0.85f, 0.25f) },
        { 29, new Color(1f, 0.53f, 0.26f) },
        { 30, new Color(0.43f, 0.75f, 1f) },
        { 31, new Color(1f, 0.6f, 0f) },
        { 32, new Color(0.88f, 0.56f, 0.9f) },
        { 34, new Color(0.6f, 0.25f, 0.75f) },
        { 1112, new Color(0.2f, 0.8f, 1f) },
        { 1113, new Color(0.7f, 0.35f, 0.98f) },
        { 1488, new Color(0.99f, 0.85f, 0.25f) }
    };

    [Description("Shop Items.")]
    public List<Preset> presets { get; set; } = new List<Preset>
    {
        new Preset("Heavy Armory", ItemType.ArmorHeavy, 250),
        new Preset("Combat Armory", ItemType.ArmorCombat, 150),
        new Preset("Light Armory", ItemType.ArmorLight, 50),
        new Preset("Medkit", ItemType.Medkit, 50),
        new Preset("Granate", ItemType.GrenadeHE, 100),
        new Preset("SCP-207", ItemType.SCP207, 350),
        new Preset("Anti-SCP-207", ItemType.AntiSCP207, 350),
        new Preset("SCP-1853", ItemType.SCP1853, 250),
        new Preset("SCP-1344", ItemType.SCP1344, 500),
        new Preset("SCP-268", ItemType.SCP268, 350),
        new Preset("Brot", 1489, 100),
        new Preset("Strahlenanzug", 6912, 750),
        new Preset("Streugranate", 26, 300),
        new Preset("Bessers SCP-207", 37, 450),
        new Preset("SCP-1499", 29, 150),
        new Preset("Nachtsichtgerät", 1113, 750),
        new Preset("SCRAMBLE-Brille", 1112, 1000),
        new Preset("Ravensinjektion [STUFE 1]", 88, 150),
        new Preset("Ravensinjektion [STUFE 2]", 89, 250),
        new Preset("Ravensinjektion [STUFE 3]", 90, 350),
    };
    
    public List<RoomType> _presetRoomTypes { get; set; } = new List<RoomType>
    {
        RoomType.LczCheckpointA,
        RoomType.LczCheckpointB,
        RoomType.HczEzCheckpointA,
        RoomType.HczEzCheckpointB,
        RoomType.Surface,
    };

    public List<RoomType> _presetRoomTypesNonNormal { get; set; } = new List<RoomType>
    {
        RoomType.LczCheckpointA,
        RoomType.LczCheckpointB,
        RoomType.HczEzCheckpointA,
        RoomType.HczEzCheckpointB,
        RoomType.Surface,
    };
    
    [Description("Crafting System.")]
    public bool CraftingSystem { get; set; } = true;
    
    [Description("List of recipes for crafting items. Format: <Input1Input2>: OutputItemType")]
        public Dictionary<string, string> Recipes { get; set; } = new()
        {
            { "RadioPainkillers", "None" },
            { "PainkillersRadio", "None" },
            { "GunCOM15Coin", "GunCOM18" },
            { "CoinGunCOM15", "GunCOM18" },
            { "GunCOM18Coin", "GunFSP9" },
            { "CoinGunCOM18", "GunFSP9" },
            { "GunFSP9Coin", "GunCrossvec" },
            { "CoinGunFSP9", "GunCrossvec" },
            { "GunCrossvecCoin", "GunE11SR" },
            { "CoinGunCrossvec", "GunE11SR" },
            { "GunE11SRCoin", "GunShotgun" },
            { "CoinGunE11SR", "GunShotgun" },
            { "GunShotgunCoin", "GunLogicer" },
            { "CoinGunShotgun", "GunLogicer" },
            { "GunLogicerCoin", "ParticleDisruptor" },
            { "CoinGunLogicer", "ParticleDisruptor" },
            { "FlashlightCoin", "GrenadeFlash" },
            { "CoinFlashlight", "GrenadeFlash" },
            { "GrenadeHEKeycardChaosInsurgency", "None" },
            { "KeycardChaosInsurgencyGrenadeHE", "None" },
            { "MedkitCoin", "Painkillers" },
            { "CoinMedkit", "Painkillers" },
            { "AdrenalinPainkiller", "SCP500" },
            { "PainkillerAdrenalin", "SCP500" },
            { "PainkillersAdrenaline", "SCP207" },
            { "AdrenalinePainkillers", "SCP207" },
            { "GunCOM15GunCOM15", "GunCom45" },
            { "RadioSCP2176", "SCP1576" },
            { "SCP2176Radio", "SCP1576" },
            { "KeycardContainmentEngineerGrenadeFlash", "Jailbird" },
            { "GrenadeFlashKeycardContainmentEngineer", "Jailbird" },
            { "GrenadeFlashFlashlight", "SCP268" },
            { "FlashlightGrenadeFlash", "SCP268" },
            { "GrenadeFlashCoin", "SCP2176" },
            { "CoinGrenadeFlash", "SCP2176" },
            { "GunCOM15SCP207", "SCP1853" },
            { "SCP207GunCOM15", "SCP1853" },
            { "JailbirdFlashlight", "ParticleDisruptor" },
            { "FlashlightJailbird", "ParticleDisruptor" },
            { "KeycardResearchCoordinatorCoinGrenadeHE", "SCP244b" },
            { "CoinGrenadeHEKeycardResearchCoordinator", "SCP244b" },
            { "CoinCoin", "Painkillers" },
            { "KeycardJanitorCoin", "KeycardScientist" },
            { "CoinKeycardJanitor", "KeycardScientist" },
            { "CoinKeycardScientist", "KeycardResearchCoordinator" },
            { "KeycardScientistCoin", "KeycardResearchCoordinator" },
            { "KeycardResearchCoordinatorCoin", "KeycardZoneManager" },
            { "CoinKeycardResearchCoordinator", "KeycardZoneManager" },
            { "CoinKeycardZoneManager", "KeycardGuard" },
            { "KeycardZoneManagerCoin", "KeycardGuard" },
            { "KeycardGuardCoin", "KeycardContainmentEngineer" },
            { "CoinKeycardGuard", "KeycardContainmentEngineer" },
            { "KeycardContainmentEngineerCoin", "KeycardMTFOperative" },
            { "CoinKeycardContainmentEngineer", "KeycardMTFOperative" },
            { "CoinKeycardKeycardMTFOperative", "KeycardMTFCaptain" },
            { "KeycardMTFOperativeCoin", "KeycardMTFCaptain" },
            { "KeycardMTFCaptainCoin", "KeycardFacilityManager" },
            { "CoinKeycardMTFCaptain", "KeycardFacilityManager" },
            { "CoinKeycardFacilityManager", "KeycardChaosInsurgency" },
            { "KeycardFacilityManagerCoin", "KeycardChaosInsurgency" },
            { "KeycardChaosInsurgencyCoin", "KeycardO5" },
            { "CoinKeycardChaosInsurgency", "KeycardO5" },
        };
    
    [Description("Crafting Hint Color.")]
    public Color CraftingHintColor { get; set; } = new Color(0.85f, 0.14f, 0.14f);
    
[Description("Should the plugin Check for Updates / Set this false if you want Custom Schematics / Sounds!")]
public bool Update { get; set; } = true;

[Description("SCP-1356 Module?")]
public bool SCP1356 { get; set; } = true;
[Description("SCP-4837 Module?")]
public bool SCP4837 { get; set; } = true;

[Description("Devnuke (Experimental Feature, for now only in German!)")]
public bool Devnuke { get; set; } = false;

[Description("Should the plugin create a backup")]
public bool Backup { get; set; } = false;

[Description("Round Summary of the Fentanyl Consumers")]
public bool RoundSummaryFentanyl { get; set; } = true;
public float RoundSummaryHintDuration { get; set; } = 10f;

[Description("Should the plugin replace a room")]
public bool ReplaceRoom { get; set; } = true;

[Description("Schematic name to replace")]
public string SchematicName { get; set; } = "FentanylReactor";

[Description("Room name to replace")]
public RoomType RoomType { get; set; } = RoomType.HczTestRoom;

[Description("Generator Settings")]
public int GeneratorMax { get; set; } = 2;

[Description("Should the plugin have an Anti-Camping Demon Core?")]
public bool DemonCore { get; set; } = true;

[Description("Demon Core Schematic Name")]
public string DemonCoreSchemeName { get; set; } = "demon_core";

[Description("Demon Core Position")]
public Vector3 DemonCorePos { get; set; } = new Vector3(27, 990.48f, -24.44f);

[Description("Demon Core Rotation")]
public Vector3 DemonCoreRot { get; set; } = new Vector3(0, 90, 0);

[Description("Demon Core Scale")]
public Vector3 DemonCoreScale { get; set; } = new Vector3(1f, 1f, 1f);

[Description("Demon Core Cooldown")]
public float DemonCoreCooldown { get; set; } = 120f;

[Description("Demon Core Color")]
public Color DemonCoreColor { get; set; } = new Color(0.5f, 0.5f, 0.5f, 0.5f);

[Description("Demon Core Color (Meltdown)")]
public Color DemonCoreColorMelt { get; set; } = new Color(57f / 255f, 255f / 255f, 20f / 255f);

[Description("Meltdown Settings")]
public bool Meltdown { get; set; } = true;

[Description("Meltdown Minimum time after round starts")]
public float MeltdownZeitMinStartRunde { get; set; } = 900f;

[Description("Meltdown Maximum time after round starts")]
public float MeltdownZeitMaxStartRunde { get; set; } = 1500f;

[Description("Minimum time to start a meltdown (subtracted from round start time)")]
public float MeltdownZeitStart { get; set; } = 10f;

[Description("Maximum time to start a meltdown (subtracted from C.A.S.S.I.E time)")]
public float MeltdownZeitEnd { get; set; } = 120f;

[Description("Should the plugin have a Hint whenever a player enters the Fentanyl Reactor?")]
public bool EnterHint { get; set; } = true;

[Description("Should the plugin use CASSIE instead of audio?")]
public bool UseCassieInsteadOfAudio { get; set; } = false;

[Description("Meltdown Color")]
public Color MeltdownColor { get; set; } = new Color(0.150f, 0, 0.40f);

[Description("Kill Area Settings")]
public float KillAreaExpansionRate { get; set; } = 1f;
public float KillAreaEffectInterval { get; set; } = 2f;
public EffectType EffectOne { get; set; } = EffectType.Poisoned;
public EffectType EffectTwo { get; set; } = EffectType.Burned;
public EffectType EffectThree { get; set; } = EffectType.Deafened;
public int EffectDuration { get; set; } = 1;
public float KillAreaDamage { get; set; } = 2f;
public float CheckInterval { get; set; } = 0.5f;

[Description("Server Specific Settings")]
public int ServerSpecificSettingHoldTime { get; set; } = 3;
public int ServerSpecificSettingId { get; set; } = 511;
public int ServerSpecificSettingIdFuel { get; set; } = 512;
public int ServerSpecificSettingIdStart { get; set; } = 513;

[Description("Fentanyl Reactor Settings")]
public float FentanylReactorAudioVolume { get; set; } = 1.0f;
public float FentanylReactorAudioMin { get; set; } = 10f;
public float FentanylReactorAudioMax { get; set; } = 20f;

[Description("Fentanyl Reactor Stage 1 success chance")]
public float Level1Chance { get; set; } = 0.75f;

[Description("Fentanyl Reactor Stage 2 success chance")]
public float Level2Chance { get; set; } = 0.55f;

[Description("Fentanyl Reactor Stage 3 success chance")]
public float Level3Chance { get; set; } = 0.25f;

[Description("Fentanyl Reactor Button Names")]
public string ButtonStage1Name { get; set; } = "Stage1";
public string ButtonStage2Name { get; set; } = "Stage2";
public string ButtonStage3Name { get; set; } = "Stage3";
public string ButtonDeomCoreName { get; set; } = "Pickup_DemonCore";
public string ButtonRefillName { get; set; } = "Refill";

[Description("Fentanyl Reactor Hints")]
public float GlobalHintDuration { get; set; } = 5f;
public int GlobalHintSize { get; set; } = 30;
public float GlobalHintY { get; set; } = 950f;

[Description("Fentanyl Reactor Timers")]
public int CommandCooldown { get; set; } = 60;
public int ReactorWaitTime { get; set; } = 10;

[Description("Fentanyl Reactor Zombification Chances")]
public float T1ZombieChance { get; set; } = 0.55F;
public float T2ZombieChance { get; set; } = 0.35F;
public float T3ZombieChance { get; set; } = 0.1F;

[Description("Fentanyl Reactor Effect Intensities")]
public byte T1Intensity { get; set; } = 1;
public byte T2Intensity { get; set; } = 1;
public byte T3Intensity { get; set; } = 1;

[Description("Fentanyl Reactor Effect Looping")]
public int T1Looping { get; set; } = 1;
public int T2Looping { get; set; } = 3;
public int T3Looping { get; set; } = 10;

[Description("Fentanyl Reactor Effect Delays")]
public float T1Delay { get; set; } = 2.5F;
public float T2Delay { get; set; } = 2.5F;
public float T3Delay { get; set; } = 2.5F;

[Description("Fentanyl Reactor Movement Speeds")]
public byte T1MovementSpeed { get; set; } = 5;
public byte T2MovementSpeed { get; set; } = 25;
public byte T3MovementSpeed { get; set; } = 50;

[Description("Fentanyl Reactor Effect Durations")]
public float T1DurationLower { get; set; } = 5;
public float T2DurationLower { get; set; } = 10;
public float T3DurationLower { get; set; } = 15;
public float T1DurationUpper { get; set; } = 10;
public float T2DurationUpper { get; set; } = 20;
public float T3DurationUpper { get; set; } = 30;

[Description("Fentanyl Reactor Custom Item IDs")]
public uint T1ID { get; set; } = 88;
public uint T2ID { get; set; } = 89;
public uint T3ID { get; set; } = 90;

[Description("Fentanyl Reactor Item Weights")]
public float T1Weight { get; set; } = 1;
public float T2Weight { get; set; } = 1;
public float T3Weight { get; set; } = 1;
}
