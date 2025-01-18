using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Fentanyl_ReactorUpdate.Configs;

public class Translation : ITranslation
{
[Description("Command Names for Administrators")]
public string CommandName { get; set; } = "FentanylReactorCore";
public string FuelCommandName { get; set; } = "FentanylReactorFuel";
public string MeltdownCommandName { get; set; } = "ForceReactorMeltdown";
public string MeltdownCancelCommandName { get; set; } = "ReactorCancelMeltdown";
public string TeleportFentanyl { get; set; } = "FentTP";

[Description("Player Permission Feedback")]
public string TeleportFentanylNoPrem { get; set; } = "You don't have the Permission to use this Button!";
public string TeleportFentanylPremission { get; set; } = "FentTP";

[Description("Hints and Messages")]
public string KillareaDeathReason { get; set; } = "Radiation";
public string DemonCoreCooldownHint { get; set; } = "The Demon Core remains in Cooldown for {DemonCoreStartCooldown} Seconds.";
public string DemonCoreAlrOpenHint { get; set; } = "The Demon Core is already unlocked.";
public string DemonCoreOpenHint { get; set; } = "The Demon Core is now unlocked.";
public string DemonCoreReadyToOpenHint { get; set; } = "The Demon Core is now able to be unlocked.";
public string EnterFentanylReactor { get; set; } = "Welcome inside the Fentanyl Reactor! \n {PlayerName}";
public string NoAdrenalineHint { get; set; } = "You have no Adrenaline!";

[Description("Fentanyl Reactor Status Hints")]
public string ReactorFueled { get; set; } = "The Fentanyl Reactor has been refueled!";
public string ReactorAlreadyFueledHint { get; set; } = "The Fentanyl Reactor is already refueled!";
public string ReactorNotFueledHint { get; set; } = "The Fentanyl Reactor is not refueled!";
public string ReactorStartingHint { get; set; } = "Fentanyl Reactor is starting...";
public string ReactorSuccessHintStageOne { get; set; } = "Fentanyl Stage One is being generated!";
public string ReactorSuccessHintStageTwo { get; set; } = "Fentanyl Stage Two is being generated!";
public string ReactorSuccessHintStageThree { get; set; } = "Fentanyl Stage Three is being generated!";
public string ReactorCooldown { get; set; } = "The Fentanyl Reactor has a cooldown of: {Cooldown} seconds";
public string ReactorFailureHint { get; set; } = "The Fentanyl Reactor could not produce anything!";

[Description("Fentanyl Round Summary Hints")]

public string RoundSummaryHint { get; set; } = "<size=30><align=right>Top Fentanyl Consumers: \n {FentanylConsumers}</size><voffset=1000> </voffset></align>";
public string RoundSummaryHintPlayers { get; set; } = "{PlayerNickname} | {FentanylItems} \n";

[Description("CASSIE Announcements")]
public string FentanylReactorMeltdownCassie { get; set; } = "pitch_0,20 .G4 . .G4 . pitch_0,95 The Reactor is overheating pitch_0,20 .G4 . .G4 . pitch_0,82 evacuate immediately pitch_0,20 .G4 . .G4 . jam_017_15 .G4";
public string FentanylReactorMeltdownCassieTrans { get; set; } = "The reactor is overheating! Evacuate immediately..";

[Description("Server Specific Settings")]
public string SSSSheaderPlayer { get; set; } = "Fentanyl Reaktor";
public string SSSSLabelFuel { get; set; } = "Fentanyl Reactor Fuel";
public string SSSSDescFuel { get; set; } = "Uses the Fentanyl Reactor Refueling";
public string SSSSFuelButton { get; set; } = "Refill";
public string SSSSStartName { get; set; } = "Fentanyl Reactor Start";
public string SSSSlStage1 { get; set; } = "Stage-1";
public string SSSSlStage2 { get; set; } = "Stage-2";
public string SSSSlStage3 { get; set; } = "Stage-3";
public string SSSStartNotInReactor { get; set; } = "You are not Inside the Fentanyl Reactor to use this!";
public string SSSSRoundNotStarted { get; set; } = "Round is not Started which is needed for this to work!";
public string SSSSPlayerIsSCP { get; set; } = "You are SCP, SCPs cannot use this!";
public string SSSSStartDesc { get; set; } = "Starts the Fentanyl Reactor on a given Stage.";

[Description("Fentanyl Stage Names and Descriptions")]
public string T1Name { get; set; } = "Fentanyl Stage 1";
public string T1Description { get; set; } = "Impure Fentanyl Stage 1!";
public string T2Name { get; set; } = "Fentanyl Stage 2";
public string T2Description { get; set; } = "Normal Fentanyl Stage 2!";
public string T3Name { get; set; } = "Fentanyl Stage 3";
public string T3Description { get; set; } = "Purest Fentanyl Stage 3!";
}
