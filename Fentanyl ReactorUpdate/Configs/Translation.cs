using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Fentanyl_ReactorUpdate.Configs;

public class Translation : ITranslation
{
    [Description("Command Name to use the Fentanyl Reactor (Admins)")]
    public string CommandName { get; set; } = "FentanylReactorCore";

    [Description("Command Name to refuel the Fentanyl Reactor (Admins)")]
    public string FuelCommandName { get; set; } = "FentanylReactorFuel";
    
    [Description("Command Name to meltdown the Fentanyl Reactor (Admins)")]
    public string MeltdownCommandName { get; set; } = "ForceReactorMeltdown";
    
    [Description("Command Name to Cancel the meltdown of the Fentanyl Reactor (Admins)")]
    public string MeltdownCancelCommandName { get; set; } = "ReactorCancelMeltdown";
    
    [Description("Command Name to Cancel the meltdown of the Fentanyl Reactor (Admins)")]
    public string TeleportFentanyl { get; set; } = "FentTP";
    [Description("Command Name to Cancel the meltdown of the Fentanyl Reactor (Admins)")]
    public string TeleportFentanylNoPrem { get; set; } = "You don't have the Premission to use this Button!";
        
    [Description("Player not in Fentanyl Reactor Hint")]
    public string FentanylReactorSSFuel { get; set; } = "You not inside the Fentanyl Reactor Basement buidling!";
    
    [Description("Fentanyl Reactor Teleport Premission")]
    public string TeleportFentanylPremission { get; set; } = "FentTP";
    
    [Description("Server Specific Settings")]
    public string SSSSheaderPlayer { get; set; } = "Fentanyl Reaktor";
    
    public string SSSSLabelTp { get; set; } = "Fentanyl Reactor Teleport";
    
    public string SSSSDescTp { get; set; } = "Uses the Fentanyl Reactor Teleport";
    
    public string SSSSTpButton { get; set; } = "TP";
    
    public string SSSSLabelFuel { get; set; } = "Fentanyl Reactor Fuel";
    
    public string SSSSDescFuel { get; set; } = "Uses the Fentanyl Reactor Refueling";
    
    public string SSSSFuelButton { get; set; } = "Refill";
    
    public string SSSSStage1Button { get; set; } = "TP";
    
    public string SSSSLabelStage1 { get; set; } = "Fentanyl Reactor Fuel";
    
    public string SSSSDescStage1 { get; set; } = "Uses the Fentanyl Reactor Refueling";
    
    public string SSSSFuelStage1 { get; set; } = "Stag";
    
    [Description("No Adrenaline Hint")]
    public string NoAdrenalineHint { get; set; } = "You have no Adrenaline!";

    [Description("Meltdown CASSIE")]
    public string FentanylReactorMeltdownCassie { get; set; } = "pitch_0,20 .G4 . .G4 . pitch_0,95 The Reactor is overheating pitch_0,20 .G4 . .G4 . pitch_0,82 evacuate immediately pitch_0,20 .G4 . .G4 . jam_017_15 .G4";

    [Description("Meltdown CASSIE Translation")]
    public string FentanylReactorMeltdownCassieTrans { get; set; } = "The reactor is overheating! Evacuate immediately..";

    [Description("Fentanyl Reactor Refuel Hint")]
    public string ReactorFueled { get; set; } = "The Fentanyl Reactor has been refueled!";

    [Description("Fentanyl Reactor already refueled hint")]
    public string ReactorAlreadyFueledHint { get; set; } = "The Fentanyl Reactor is already refueled!";

    [Description("Fentanyl Reactor not refueled hint")]
    public string ReactorNotFueledHint { get; set; } = "The Fentanyl Reactor is not refueled!";

    [Description("Fentanyl Reactor Starting Hint")]
    public string ReactorStartingHint { get; set; } = "Fentanyl Reactor is starting...";

    [Description("Fentanyl Reactor Success Hint Stage 1")]
    public string ReactorSuccessHintStageOne { get; set; } = "Fentanyl Stage One is being generated!";

    [Description("Fentanyl Reactor Success Hint Stage 2")]
    public string ReactorSuccessHintStageTwo { get; set; } = "Fentanyl Stage Two is being generated!";

    [Description("Fentanyl Reactor Success Hint Stage 3")]
    public string ReactorSuccessHintStageThree { get; set; } = "Fentanyl Stage Three is being generated!";

    [Description("Fentanyl Reactor Cooldown Hint")]
    public string ReactorCooldown { get; set; } = "The Fentanyl Reactor has a cooldown of: %Cooldown% seconds";

    [Description("Fentanyl Reactor could not produce anything hint")]
    public string ReactorFailureHint { get; set; } = "The Fentanyl Reactor could not produce anything!";
    [Description("Fentanyl Stage 1 Name")]
    public string T1Name { get; set; } = "Fentanyl Stage 1"; 
    [Description("Fentanyl Stage 1 Description")]
    public string T1Description { get; set; } = "Impure Fentanyl Stage 1!"; 
    [Description("Fentanyl Stage 2 Name")]
    public string T2Name { get; set; } = "Fentanyl Stage 2"; 
    [Description("Fentanyl Stage 2 Description")]
    public string T2Description { get; set; } = "Normal Fentanyl Stage 2!"; 
    [Description("Fentanyl Stage 3 Name")]
    public string T3Name { get; set; } = "Fentanyl Stage 3"; 
    [Description("Fentanyl Stage 3 Description")]
    public string T3Description { get; set; } = "Purest Fentanyl Stage 3!";  
}
