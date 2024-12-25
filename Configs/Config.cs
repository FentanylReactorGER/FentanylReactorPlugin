using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;

namespace Fentanyl_ReactorUpdate.Configs;

public class Config : IConfig
{
    [Description("Soll das Plugin Aktiviert werden")]
        
    public bool IsEnabled { get; set; } = true;
        
    [Description("Soll das Plugin einen Debug anzeigen")]
    public bool Debug { get; set; } = false;
    
    [Description("Soll das Plugin ein Backup erstellen")]
    public bool Backup { get; set; } = false;
    
    [Description("Plugin Path")]
    public string PluginPath { get; set; } = "/home/container/.config/EXILED/Plugins/UsefulHints.dll";
        
    [Description("Soll das Plugin einen Raum Replacen")]
    public bool ReplaceRoom { get; set; } = true;
        
    [Description("Schematic Name zum Replacen")]
    public string SchematicName { get; set; } = "FentanylReactor";

    [Description("Raum Name zum Replacen")]
    public RoomType RoomType { get; set; } = RoomType.HczTestRoom;
        
    [Description("Meltdown Zeit nach Start der Runde")]
    public float MeltdownZeitStartRunde { get; set; } = 1500f;

    [Description("Meltdown Minimale Zeit zum Starten eines Meltdowns (nach der Rundenstartzeit abgezogen)")]
    public float MeltdownZeitStart { get; set; } = 10f;
    [Description("Meltdown Maximale Zeit zum Starten eines Meltdowns (nach der Rundenstartzeit abgezogen)")]
    public float MeltdownZeitEnd { get; set; } = 120f;

    [Description("Fentanyl Reaktor Stufe 1 Erfolgschance")]
    public float Level1Chance { get; set; } = 0.75f;
    [Description("Fentanyl Reaktor Stufe 2 Erfolgschance")]
    public float Level2Chance { get; set; } = 0.55f;
    [Description("Fentanyl Reaktor Stufe 3 Erfolgschance")]
    public float Level3Chance { get; set; } = 0.25f;
        
    [Description("Fentanyl Reaktor Audio Lautstärke")]
    public float FentanylReactorAudioVolume { get; set; } = 1.0f;
        
    [Description("Fentanyl Reaktor Audio stufe 1")]
    public string Level1AudioPath { get; set; } = "/home/zap1208577/g648626/.config/EXILED/Plugins/audio/FentReactorTest.ogg";
    [Description("Fentanyl Reaktor Audio stufe 2")]
    public string Level2AudioPath { get; set; } = "/home/zap1208577/g648626/.config/EXILED/Plugins/audio/FentReactorTest.ogg";
    [Description("Fentanyl Reaktor Audio stufe 3")]
    public string Level3AudioPath { get; set; } = "/home/zap1208577/g648626/.config/EXILED/Plugins/audio/FentReactorTest.ogg";
        
    [Description("Fentanyl Reaktor kein Fentanyl Audio")]
    public string FailureAudioPath { get; set; } = "/home/zap1208577/g648626/.config/EXILED/Plugins/audio/FentReactorTest.ogg";
        
    [Description("Fentanyl Reaktor Fentanyl Stufe 1 Button Name")]
    public string ButtonStage1Name { get; set; } = "Stage1";
        
    [Description("Fentanyl Reaktor Fentanyl Stufe 1 Button Name")]
    public string ButtonStage2Name { get; set; } = "Stage2";
        
    [Description("Fentanyl Reaktor Fentanyl Stufe 1 Button Name")]
    public string ButtonStage3Name { get; set; } = "Stage3";
        
    [Description("Fentanyl Reaktor Fentanyl Stufe 1 Button Name")]
    public string ButtonRefillName { get; set; } = "Refill";

    [Description("Fentanyl Reaktor Globalen Hints dauer")]
    public float GlobalHintDuration { get; set; } = 5f;

    [Description("Fentanyl Reaktor Command Cooldown")]
    public int CommandCooldown { get; set; } = 60;
    [Description("Fentanyl Reaktor Wartezeit bis zum Produkt")]
    public int ReactorWaitTime { get; set; } = 10;

    [Description("Die Zombifizierungschancen jeder Stufe von Fentanyl")]
    public float T1ZombieChance { get; set; } = 0.55F;
    public float T2ZombieChance { get; set; } = 0.35F;
    public float T3ZombieChance { get; set; } = 0.1F;

    [Description("Die Änderung der Intensität jedes Effekts für jede Stufe von Fentanyl")]
    public byte T1Intensity { get; set; } = 1;
    public byte T2Intensity { get; set; } = 1;
    public byte T3Intensity { get; set; } = 1;

    [Description("Die Anzahl der Male, die jede Stufe von Fent die Intensität (um ihre jeweilige Intensität) eines Effekts erhöht")]
    public int T1Looping { get; set; } = 1;
    public int T2Looping { get; set; } = 3;
    public int T3Looping { get; set; } = 10;

    [Description("Das Delay zwischen der nutztung und der Effekte des Fentanyls")]
    public float T1Delay { get; set; } = 2.5F;
    public float T2Delay { get; set; } = 2.5F;
    public float T3Delay { get; set; } = 2.5F;

    [Description("Die änderung an Geschwindigkeit des Fentanyls")]
    public byte T1MovementSpeed { get; set; } = 5;
    public byte T2MovementSpeed { get; set; } = 25;
    public byte T3MovementSpeed { get; set; } = 50;
        
    [Description("Die Minimale dauer der Effekte des Fentanyls")]
    public float T1DurationLower { get; set; } = 5;
    public float T2DurationLower { get; set; } = 10;
    public float T3DurationLower { get; set; } = 15;
        
    [Description("Die Maximale dauer der Effekte des Fentanyls")]
    public float T1DurationUpper { get; set; } = 10;
    public float T2DurationUpper { get; set; } = 20;
    public float T3DurationUpper { get; set; } = 30;

    [Description("Die Custom Item ID für das Fentanyl Item")]
    public uint T1ID { get; set; } = 88;
    public uint T2ID { get; set; } = 89;
    public uint T3ID { get; set; } = 90;

    [Description("Weight von jedem Fentanyl Item.")]
    public float T1Weight { get; set; } = 1;
    public float T2Weight { get; set; } = 1;
    public float T3Weight { get; set; } = 1;
}