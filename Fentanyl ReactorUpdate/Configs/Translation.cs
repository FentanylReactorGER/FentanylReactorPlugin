using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using Fentanyl_ReactorUpdate.API.SCP4837;
using PlayerRoles;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.Configs;


public class Translation : ITranslation
{
    
[Description("Command Names for Administrators")]
public string CommandName { get; set; } = "FentanylReactorCore";
public string FuelCommandName { get; set; } = "FentanylReactorFuel";
public string MeltdownCommandName { get; set; } = "ForceReactorMeltdown";
public string MeltdownCancelCommandName { get; set; } = "ReactorCancelMeltdown";
public string TeleportFentanyl { get; set; } = "FentTP";

[Description("Hints and Messages")]
public string KillareaDeathReason { get; set; } = "Radiation";
public string DemonCoreCooldownHint { get; set; } = "The Demon Core remains in Cooldown for {DemonCoreStartCooldown} Seconds.";
public string DemonCoreAlrOpenHint { get; set; } = "The Demon Core is already unlocked.";
public string DemonCoreOpenHint { get; set; } = "The Demon Core is now unlocked.";
public string DemonCoreReadyToOpenHint { get; set; } = "The Demon Core is now able to be unlocked.";

public string EnterCraftingMenu { get; set; } = "Das ist ein <b>Crafting-Menü</b>! \n" +
                                                "Unter <b>ESC ➜ Einstellungen ➜ Server-Spezifisch ➜ Raven's Garden</b> \n" +
                                                "kannst du mehr darüber erfahren, {PlayerName}!";
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
    public string Scp4837HintDisplay { get; set; } = "Soll der SCP-4837 Tutorial Hint angezeigt werden?";
    public string Scp4837CustomSounds { get; set; } = "Sollen Custom SCP Sounds gespielt werden?";
    public string Scp4837CustomSoundsDescription { get; set; } = "Legt fest, ob für SCP-4837 benutzerdefinierte Geräusche abgespielt werden sollen. Diese Sounds können das Spielerlebnis einzigartiger und immersiver machen.";
    public string Scp4837JumpScareSounds { get; set; } = "Soll SCP-4837 Jumpscare Sounds spielen?";
    public string Scp4837PocketDimensionColor { get; set; } = "SCP-4837 Pocket Dimensions Farbe";
    public string Scp4837PocketDimensionColorDescription { get; set; } = "Wähl deine Eigene Farbe aus, für die Dimension von SCP-4837!";
    public string Scp4837RainbowColors { get; set; } = "Soll SCP-4837 Regenbogen-Farben nutzen?";
    public string Scp4837RainbowColorsDescription { get; set; } = "Dies überschreibt deine Eigende Farbe, falls es Aktiv ist!";
    public string Scp4837CustomInteractionKey { get; set; } = "Sollen Custom Interaktionen den 'ALT' Key nutzen?";
    public string Scp4837CustomInteractionKeyDescription { get; set; } = "Dies überschreibt NICHT deinen Custom Keybind!";
    public string Scp4837CustomInteraction { get; set; } = "Custom Interaktion";
    
    [Description("Server Specific Settings Custom Colors")]
    public Dictionary<string, Color> Presets { get; set; } = new()
    {
        { "Weiß", Color.white },
        { "Schwarz", Color.black },
        { "Grau", Color.gray },
        { "Rot", Color.red },
        { "Grün", Color.green },
        { "Blau", Color.blue },
        { "Gelb", Color.yellow },
        { "Cyan", Color.cyan },
        { "Magenta", Color.magenta },
    };
    
    public string ServerDescriptionHeader { get; set; } = "<b>Server Custom Map Beschreibung</b>";
    public string RavensReactorDescription { get; set; } = ">Dies ist die Beschreibung für <b>Raven's Garden</b>!";
    public string RavensReactorDetails { get; set; } = "Der <b>Ravensreaktor</b> ist ein Gebäude, das in der Heavy Containment Zone spawnt.</color>\nDort verfügt es über einen <b>Reaktor</b>, der eine Ravensinjektion herstellen kann. \nDafür muss dieser im Erdgeschoss mit einem <b>Adrenaline</b> gefüllt werden. \nDem Nutzer stehen dann <b>3 Stufen</b> zur Auswahl, jeweils eine besser und seltener als die andere.\nIm oberen Gebäude spawnt dann <b><color=#32CD32>SCP-1356</color></b>, Medkits, <b>SCP-500s</b>, eine <b>COM-15</b> und mit Glück eine <b>O5-Keycard</b>.";
    public string DuckUnitDescription { get; set; } = "Die<b>D.U.C.K</b></color> ist eine Custom Unit, die mit der <b>Chaos Insurgency</b> kooperiert.\n Ihr Ziel ist die 'Befreiung' des Foundation-Personals vor dessen 'Unheil' mit der Befreiung von <b>SCP-1356</b>.\nDafür arbeiten sie mit den SCPs zusammen und können jedes Foundation-Personal <b>fesseln</b> und auf ihre Seite zwingen! \nDer <b>D.U.C.K Eindämmer</b> verfügt über ein Eindämmungsgerät, welches <b>SCP-1356</b> mit Standard: 'ALT' eindämmt und einen zweiten Respawn sichert!";
    public string Scp4837Description { get; set; } = "<b>SCP-4837</b> ist ein anomaler Rabe, der unten bei <b>SCP-049</b> in seiner Armory spawnt.</color>\n Er handelt wertvolle Items gegen Brot, das in der Facility auffindbar ist.\nWenn man sich <b>SCP-4837</b> mit Brot nähert, kann man mit Standard: 'ALT' einen Handel starten.";
    public string Scp1356Description { get; set; } = "<b>SCP-1356</b> ist eine anomale Ente, die im obersten Geschoss des Raven's Reaktor spawnt.</color>\n Sie ist augenscheinlich <b>gelb</b> und charakteristisch friedlich.\nSie wird jedoch gefährlich, wenn man sich ihr ohne <b>Strahlenanzug</b> nähert, was zu Strahlenverletzungen führt.\nSie ist auch das erste Ziel der <b>D.U.C.K</b>, denn durch <b>SCP-1356</b> kann diese einen <b>zweiten Respawn</b> bekommen! \n Nach 15 Minuten, falls nicht eingedämmt durch Waffen oder geklaut durch die <b>D.U.C.K</b>, wird sie <b>breachen</b> und Schande über euch bringen!";
    public string SSHeaderMain { get; set; } = "Raven's garden Interaktion";
    public string SSHeaderCraftSystem { get; set; } = "Raven's garden Crafting System";
    public string CraftingSystemDescription { get; set; } = "Das Crafting System";
    public string CraftingSystemListRecipies { get; set; } = "Crafting System Rezepte";
    public string CraftingSystemListRecipiesHnt { get; set; } = "Crafting System Rezepte";
    public string ItemDropdownLabel { get; set; } = "Gegenstand";
    
    public string ConfirmPurchaseButtonLabel { get; set; } = "Kauf bestätigen";
    public string ConfirmPurchaseButtonAction { get; set; } = "Kaufen";

    public string CheckBalanceButtonLabel { get; set; } = "Kontostand Checken";
    public string CheckBalanceButtonAction { get; set; } = "Checken";

    [Description("SCP-1356 Translations")]
    public string SCP1356ContainedGlassHint { get; set; } = "⚠️ <b>SCP-1356 ist noch eingeschlossen!</b> \n" + "🎯 Schieß auf das <b>dunklere Glas</b>, um das Containment zu öffnen. \n" + "🦆 Anschließend drücke Standart: <b>ALT</b> auf die Ente, um fortzufahren!";
    public string SCP1356ContainedGlassOpenHint { get; set; } = "✅ <b>SCP-1356 ist frei!</b> \n" + "🦆 Drücke Standart: <b>ALT</b> auf die Ente, um der D.U.C.K einen Respawn zu sichern!";
    public string SCP1356TooFarHint { get; set; } = "<color=yellow>⚠️</color> Du bist zu weit von <b><color=#f5eb09>SCP-1356</b></color> entfernt!";
    public string SCP1356CapturedHint { get; set; } = "Du hast SCP-1356 befreit und eine D.U.C.K Spawnwelle erzwungen!";
    public string SCP1356DeviceRequiredHint { get; set; } = "Du musst dein Eindämmungsgerät in der Hand halten, um SCP-1356 zu fangen!";
    public string SCP1356InvalidRoleHint { get; set; } = "Du bist kein Mitglied der D.U.C.K und kannst SCP-1356 nicht fangen!\n Wie zur Hölle lebst du noch?";
    public string SCP1356DuckWaveHint { get; set; } = "D.U.C.K Spawnwelle wurde erzwungen";
    
    public string SCP1356CassieMessage { get; set; } = "pitch_0.7 BELL_START .g4 .g6 pitch_1 SCP 1 3 5 6 got pitch_0.9 captured pitch_1 by D U C K intruders . . continue with pitch_0.9 caution pitch_1 pitch_0.7 .g6 .g4 BELL_END pitch_1";
    public string SCP1356CassieMessageTranslated { get; set; } = "<color=yellow>SCP-1356</color> wurde von der <color=yellow>D.U.C.K</color> beschlagnahmt. Fahren Sie mit <color=red>Vorsicht</color> fort!";
    
    public string SCP1356CassieMessageBreach { get; set; } = "pitch_0.7 BELL_START .g4 .g6 pitch_1 SCP 1 3 5 6 containment pitch_0.9 failure pitch_1 detected . Containment status .g3 .g2 pitch_0.7 error .g2 . pitch_1 . continue with pitch_0.9 caution pitch_1 pitch_0.7 .g6 .g4 BELL_END pitch_1";
    public string SCP1356CassieMessageTranslatedBreach { get; set; } = "<color=yellow>SCP-1356</color> Eindämmungsbruch festgestellt. Eindämmungsstatus: <color=red>Fehler</color>. Fahren Sie mit <color=red>Vorsicht</color> fort!";

    public string SCP1356CassieMessageContainNtf { get; set; } = "SCP 1 3 5 6 contain successfully . Containmentunit {unitDesignation} {unitNumber}";
    public string SCP1356CassieMessageContainOther { get; set; } = "SCP 1 3 5 6 contain successfully by {customTranslation}";

    public string SCP1356CassieMessageTranslatedContainNtf { get; set; } = "SCP-1356 Eindämmung erfolgreich. Eindämmungseinheit: {unitDesignation}.";
    public string SCP1356CassieMessageTranslatedContainOther { get; set; } = "SCP-1356 Eindämmung erfolgreich durch {customTranslationGer}.";

    public string SCP1356CantContainRole { get; set; } = "Als D.U.C.K kannst du SCP-1356 nicht Eindämmen!";
    public string SCP1356MoneyHintContain { get; set; } = "Für diese Eindämmung von SCP-1356 hast du 50 Solana erhalten! \n Dein Geld: {0} Solana!";
    
    public Dictionary<RoleTypeId, string> RoleTranslations { get; set; } =  new()
    {
        { RoleTypeId.ClassD, "Class D Personnel" },
        { RoleTypeId.Scientist, "Scientist Personnel" },
        { RoleTypeId.ChaosConscript, "Chaos Insurgency" },
        { RoleTypeId.ChaosMarauder, "Chaos Insurgency" },
        { RoleTypeId.ChaosRepressor, "Chaos Insurgency" },
        { RoleTypeId.ChaosRifleman, "Chaos Insurgency" }
    };
    
    public Dictionary<RoleTypeId, string> RoleTranslationsCustomLanguage { get; set; } =  new()
    {
        { RoleTypeId.ClassD, "Klasse-D Personal" },
        { RoleTypeId.Scientist, "Wissenschaftliches Personal" },
        { RoleTypeId.ChaosConscript, "Chaos Insurgency" },
        { RoleTypeId.ChaosMarauder, "Chaos Insurgency" },
        { RoleTypeId.ChaosRepressor, "Chaos Insurgency" },
        { RoleTypeId.ChaosRifleman, "Chaos Insurgency" }
    };
    
    [Description("SCP-4837 Translations")]
    public string SCP4837MaxTrades { get; set; } = "<color=yellow>\u26a0\ufe0f</color> Du hast bereits dreimal mit <b><color=#757935>SCP-4837</b></color> gehandelt.\n Du kannst nächste Runde erneut handeln!";
    public string SCP4837TradeTimer { get; set; } = "Du hast noch <b>{Scp4837TradeDur} Sekunden</b>, um mit <b><color=#757935>SCP-4837</b></color> zu traden!";
    public string SCP4837TradeTimerOver { get; set; } = "Die Zeit ist abgelaufen! Du wurdest zurück <b>Teleportiert</b>.";
    public string SCP4837CooldownHint { get; set; } = "<color=yellow>⚠️</color> <b><color=#757935>SCP-4837</b></color> hat Cooldown!";
    public string SCP4837TooFarHint { get; set; } = "<color=yellow>⚠️</color> Du bist zu weit von <b><color=#757935>SCP-4837</b></color> entfernt!";
    public string SCP4837ItemRequiredHint { get; set; } = "<color=yellow>⚠️</color> Du musst <b>Brot</b> in der Hand halten, um mit <b><color=#757935>SCP-4837</b></color> zu handeln!";

    [Description("Money Translations")] 
    public string SCP4837TradeMoney { get; set; } = "Für diese Interaktion mit SCP-4837 hast du 15 Solana erhalten!";
    public string SCP1356RewardHint { get; set; } = "Für diesen Fang von SCP-1356 hast du 50 Solana erhalten!\n Dein Geld: {0} Solana!";
public string T1Name { get; set; } = "Fentanyl Stage 1";
public string T1Description { get; set; } = "Impure Fentanyl Stage 1!";
public string T2Name { get; set; } = "Fentanyl Stage 2";
public string T2Description { get; set; } = "Normal Fentanyl Stage 2!";
public string T3Name { get; set; } = "Fentanyl Stage 3";
public string T3Description { get; set; } = "Purest Fentanyl Stage 3!";
}
