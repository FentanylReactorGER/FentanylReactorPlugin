using System;
using CommandSystem;
using Exiled.API.Features;
using UnifiedEconomy.Helpers.Extension;

namespace Fentanyl_ReactorUpdate.API.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MONEYYY : ICommand
    {
        public string Command => "MoneyForPlayer";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Verwalte das Geld eines Spielers (ADD / REMOVE / CHECK).";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Argumentanzahl prüfen
            if (arguments.Count < 2)
            {
                response = "Usage: MoneyForPlayer <Spieler> <ADD / REMOVE / CHECK> <SUMME>";
                return false;
            }

            // Spieler-ID aus Argumenten lesen und validieren
            if (!int.TryParse(arguments.At(0), out int playerId))
            {
                response = "Ungültige Spieler-ID! Bitte eine gültige Spieler-ID eingeben.";
                return false;
            }

            // Spieler finden
            Player player = Player.Get(playerId);
            if (player == null)
            {
                response = $"Spieler mit der ID {playerId} konnte nicht gefunden werden!";
                return false;
            }

            // Aktion (ADD / REMOVE / CHECK) lesen und validieren
            string action = arguments.At(1).ToUpper();
            if (action != "ADD" && action != "REMOVE" && action != "CHECK")
            {
                response = "Ungültige Aktion! Erlaubt sind: ADD, REMOVE, CHECK.";
                return false;
            }

            // Aktionen ADD und REMOVE prüfen (benötigen 3 Argumente)
            if ((action == "ADD" || action == "REMOVE") && arguments.Count < 3)
            {
                response = "Usage: MoneyForPlayer <Spieler> <ADD / REMOVE> <SUMME> | Du hast die Summe vergessen!";
                return false;
            }

            // Geldbetrag lesen und validieren (nur für ADD und REMOVE)
            float amount = 0;
            if ((action == "ADD" || action == "REMOVE") && !float.TryParse(arguments.At(2), out amount))
            {
                response = "Ungültige Summe! Bitte eine gültige Zahl eingeben.";
                return false;
            }

            // Aktionen ausführen
            switch (action)
            {
                case "ADD":
                    player.AddBalance(amount);
                    response = $"Es wurden {amount} {Plugin.Singleton.WebSocketServer.GetCustomMessage(player.UserId)} zu {player.Nickname} hinzugefügt!";
                    return true;

                case "REMOVE":
                    player.RemoveBalance(amount);
                    response = $"Es wurden {amount} {Plugin.Singleton.WebSocketServer.GetCustomMessage(player.UserId)} von {player.Nickname} entfernt!";
                    return true;

                case "CHECK":
                    float balance = player.GetPlayerFromDB().Balance;
                    response = $"{player.Nickname} hat {balance} {Plugin.Singleton.WebSocketServer.GetCustomMessage(player.UserId)}!";
                    return true;

                default:
                    response = "Unbekannter Fehler!";
                    return false;
            }
        }
    }
}
