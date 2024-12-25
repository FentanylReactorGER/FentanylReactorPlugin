using System.ComponentModel;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Interfaces;

namespace Fentanyl_ReactorUpdate.Configs;

public class Config : IConfig
{
    [Description("Should the plugin be enabled")]
    public bool IsEnabled { get; set; } = true;
        
    [Description("Should the plugin display a debug message")]
    public bool Debug { get; set; } = false;
    
    [Description("Should the plugin create a backup")]
    public bool Backup { get; set; } = false;
        
    [Description("Should the plugin replace a room")]
    public bool ReplaceRoom { get; set; } = true;
        
    [Description("Schematic name to replace")]
    public string SchematicName { get; set; } = "FentanylReactor";

    [Description("Room name to replace")]
    public RoomType RoomType { get; set; } = RoomType.HczTestRoom;
        
    [Description("Meltdown time after round starts")]
    public float MeltdownZeitStartRunde { get; set; } = 1500f;

    [Description("Minimum time to start a meltdown (subtracted from round start time)")]
    public float MeltdownZeitStart { get; set; } = 10f;

    [Description("Maximum time to start a meltdown (subtracted from round start time)")]
    public float MeltdownZeitEnd { get; set; } = 120f;

    [Description("Fentanyl Reactor Stage 1 success chance")]
    public float Level1Chance { get; set; } = 0.75f;

    [Description("Fentanyl Reactor Stage 2 success chance")]
    public float Level2Chance { get; set; } = 0.55f;

    [Description("Fentanyl Reactor Stage 3 success chance")]
    public float Level3Chance { get; set; } = 0.25f;
        
    [Description("Fentanyl Reactor audio volume")]
    public float FentanylReactorAudioVolume { get; set; } = 1.0f;
    
    [Description("Fentanyl Reactor audio Distance Min")]
    public float FentanylReactorAudioMin { get; set; } = 10f;
    
    [Description("Fentanyl Reactor audio Distance Max")]
    public float FentanylReactorAudioMax { get; set; } = 20f;
        
    [Description("Fentanyl Reactor Fentanyl stage 1 button name")]
    public string ButtonStage1Name { get; set; } = "Stage1";
        
    [Description("Fentanyl Reactor Fentanyl stage 2 button name")]
    public string ButtonStage2Name { get; set; } = "Stage2";
        
    [Description("Fentanyl Reactor Fentanyl stage 3 button name")]
    public string ButtonStage3Name { get; set; } = "Stage3";
        
    [Description("Fentanyl Reactor refill button name")]
    public string ButtonRefillName { get; set; } = "Refill";

    [Description("Fentanyl Reactor global hint duration")]
    public float GlobalHintDuration { get; set; } = 5f;

    [Description("Fentanyl Reactor command cooldown")]
    public int CommandCooldown { get; set; } = 60;

    [Description("Fentanyl Reactor wait time until product")]
    public int ReactorWaitTime { get; set; } = 10;

    [Description("The zombification chance for each Fentanyl stage")]
    public float T1ZombieChance { get; set; } = 0.55F;

    public float T2ZombieChance { get; set; } = 0.35F;

    public float T3ZombieChance { get; set; } = 0.1F;

    [Description("The change in intensity for each effect at each Fentanyl stage")]
    public byte T1Intensity { get; set; } = 1;

    public byte T2Intensity { get; set; } = 1;

    public byte T3Intensity { get; set; } = 1;

    [Description("The number of times each Fentanyl stage increases the intensity of an effect (by its respective intensity)")]
    public int T1Looping { get; set; } = 1;

    public int T2Looping { get; set; } = 3;

    public int T3Looping { get; set; } = 10;

    [Description("The delay between the usage and the effects of Fentanyl")]
    public float T1Delay { get; set; } = 2.5F;

    public float T2Delay { get; set; } = 2.5F;

    public float T3Delay { get; set; } = 2.5F;

    [Description("The change in movement speed due to Fentanyl")]
    public byte T1MovementSpeed { get; set; } = 5;

    public byte T2MovementSpeed { get; set; } = 25;

    public byte T3MovementSpeed { get; set; } = 50;
        
    [Description("The minimum duration of Fentanyl effects")]
    public float T1DurationLower { get; set; } = 5;

    public float T2DurationLower { get; set; } = 10;

    public float T3DurationLower { get; set; } = 15;
        
    [Description("The maximum duration of Fentanyl effects")]
    public float T1DurationUpper { get; set; } = 10;

    public float T2DurationUpper { get; set; } = 20;

    public float T3DurationUpper { get; set; } = 30;

    [Description("The custom item ID for the Fentanyl item")]
    public uint T1ID { get; set; } = 88;

    public uint T2ID { get; set; } = 89;

    public uint T3ID { get; set; } = 90;

    [Description("The weight of each Fentanyl item")]
    public float T1Weight { get; set; } = 1;

    public float T2Weight { get; set; } = 1;

    public float T3Weight { get; set; } = 1;
}
