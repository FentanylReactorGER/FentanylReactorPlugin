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

    [Description("No Adrenaline Hint")]
    public string NoAdrenalineHint { get; set; } = "You have no adrenaline!";

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
