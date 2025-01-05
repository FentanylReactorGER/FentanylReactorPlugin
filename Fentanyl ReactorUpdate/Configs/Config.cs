using System.ComponentModel;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PluginAPI.Core;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.Configs;

public class Config : IConfig
{
[Description("Should the plugin be enabled")]
public bool IsEnabled { get; set; } = true;

[Description("Should the plugin display a debug message")]
public bool Debug { get; set; } = false;

[Description("Should the plugin Check for Updates / Set this false if you want Custom Schematics / Sounds!")]
public bool Update { get; set; } = true;

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
