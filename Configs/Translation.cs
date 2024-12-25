using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Fentanyl_ReactorUpdate.Configs;

public class Translation : ITranslation
{
    [Description("Command Name zum Nutzen des Fentanyl Reaktors (Admins)")]
    public string CommandName { get; set; } = "FentanylReactorCore";

    [Description("Command Name zum Befüllen des Fentanyl Reaktors (Admins)")]
    public string FuelCommandName { get; set; } = "FentanylReactorFuel";

    [Description("Kein Adrenaline Hinweis")]
    public string NoAdrenalineHint { get; set; } = "Du hast kein Adrenalin!";

    [Description("Meltdown CASSIE")]
    public string FentanylReactorMeltdownCassie { get; set; } = "pitch_0,20 .G4 . .G4 . pitch_0,95 The Reactor is overheating pitch_0,20 .G4 . .G4 . pitch_0,82 evacuate immediately pitch_0,20 .G4 . .G4 . jam_017_15 .G4";

    [Description("Meltdown CASSIE Übersetzung")]
    public string FentanylReactorMeltdownCassieTrans { get; set; } = "Der Reaktor überhitzt! Sofort EVAKUIEREN..";

    [Description("Fentanyl Reaktor Nachfüllhinweis")]
    public string ReactorFueled { get; set; } = "Der Fentanyl Reaktor wurde aufgefüllt!";

    [Description("Fentanyl Reaktor ist bereits aufgefüllt Hinweis")]
    public string ReactorAlreadyFueledHint { get; set; } = "Der Fentanyl Reaktor ist bereits aufgefüllt!";

    [Description("Fentanyl Reaktor ist nicht aufgefüllt Hinweis")]
    public string ReactorNotFueledHint { get; set; } = "Der Fentanyl Reaktor ist nicht aufgefüllt!";

    [Description("Fentanyl Reaktor Startet Hinweis")]
    public string ReactorStartingHint { get; set; } = "Fentanyl Reaktor startet...";

    [Description("Fentanyl Reaktor Erfolg Hinweis Stufe 1")]
    public string ReactorSuccessHintStageOne { get; set; } = "Fentanyl Stufe Eins wird generiert!";

    [Description("Fentanyl Reaktor Erfolg Hinweis Stufe 2")]
    public string ReactorSuccessHintStageTwo { get; set; } = "Fentanyl Stufe Zwei wird generiert!";

    [Description("Fentanyl Reaktor Erfolg Hinweis Stufe 3")]
    public string ReactorSuccessHintStageThree { get; set; } = "Fentanyl Stufe Drei wird generiert!";

    [Description("Fentanyl Reaktor Cooldown Hinweis")]
    public string ReactorCooldown { get; set; } = "Der Fentanyl Reaktor hat eine Abklingzeit von: %Cooldown% Sekunden";

    [Description("Fentanyl Reaktor konnte nichts produzieren Hinweis")]
    public string ReactorFailureHint { get; set; } = "Der Fentanyl Reaktor konnte nichts produzieren!";
    [Description("Fentanyl Stufe 1 Name")]
    public string T1Name { get; set; } = "Fentanyl Stufe 1"; 
    [Description("Fentanyl Stufe 1 Beschreibung")]
    public string T1Description { get; set; } = "Unreines Fentanyl Stufe 1!"; 
    [Description("Fentanyl Stufe 2 Name")]
    public string T2Name { get; set; } = "Fentanyl Stufe 2"; 
    [Description("Fentanyl Stufe 2 Beschreibung")]
    public string T2Description { get; set; } = "Normales Fentanyl Stufe 2!"; 
    [Description("Fentanyl Stufe 3 Name")]
    public string T3Name { get; set; } = "Fentanyl Stufe 3"; 
    [Description("Fentanyl Stufe 3 Beschreibung")]
    public string T3Description { get; set; } = "Reinstes Fentanyl Stufe 3!";  
}