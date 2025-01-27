using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Fentanyl_ReactorUpdate.API.SCP4837
{
    public class PlayerColorManager
    {
        private readonly Dictionary<Player, Color> playerColorSelections = new();
        
        public void SetPlayerColor(Player player, Color selectedColor, SSTextArea text)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            
            playerColorSelections[player] = selectedColor;
            
            string hexColor = ColorUtility.ToHtmlStringRGB(selectedColor);
            text.SendTextUpdate($"Du hast deine Farbe <color=#{hexColor}>geändert</color>!");
            Log.Info($"{player.Nickname} hat die Farbe {hexColor} ausgewählt.");
        }
        
        public Color GetPlayerColor(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            return playerColorSelections.TryGetValue(player, out var color) ? color : Color.white;
        }
    }
}