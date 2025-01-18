using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using InventorySystem.Items;
using MapEditorReborn.API.Features.Objects;
using ServerSpecificSyncer.Features;
using ServerSpecificSyncer.Features.Wrappers;
using UnityEngine;
using UserSettings.ServerSpecific;
using MEC;
using PlayerRoles;
using UserSettings.ServerSpecific.Examples;

namespace Fentanyl_ReactorUpdate.API.SCP4837
{
    public class SCP4837InteractionMenu : Menu
    {
        private const string Scp4837InteractionHint = "SCP-4837 Interaktionen.";
        private List<ServerSpecificSettingBase> _settings;
        private readonly Dictionary<Player, float> _lastHintTimes = new();
        private List<Preset> _presets;

        public class Preset
        {
            public string Name { get; set; }
            public Color Color { get; set; }

            public Preset(string name, Color color)
            {
                Name = name;
                Color = color;
            }
        }

        public override ServerSpecificSettingBase[] Settings => GetSettings();

        private ServerSpecificSettingBase[] GetSettings()
        {
            _presets ??= new List<Preset>
            {
                new Preset("Weiß", Color.white),
                new Preset("Schwarz", Color.black),
                new Preset("Grau", Color.gray),
                new Preset("Rot", Color.red),
                new Preset("Grün", Color.green),
                new Preset("Blau", Color.blue),
                new Preset("Gelb", Color.yellow),
                new Preset("Cyan", Color.cyan),
                new Preset("Magenta", Color.magenta),
            };
            
            _settings = new List<ServerSpecificSettingBase>
            {
                new YesNoButton(412, "Soll der SCP-4837 Tutorial Hint angezeigt werden?", "Ja", "Nein", OnAvoid4837Hint, false),
                new YesNoButton(413, "Sollen Custom SCP Sounds gespielt werden?", "Ja", "Nein", OnAvoid4837Hint, false),
                new YesNoButton(414, "Soll SCP-4837 Jumpscare Sounds spielen?", "Ja", "Nein", OnAvoid4837Hint, false),
                new Dropdown(415, "SCP-4837 Pocket Dimensions Farbe", _presets.Select(x => x.Name).ToArray(), Chose4837Color, 1, SSDropdownSetting.DropdownEntryType.Hybrid, "Wähl deine Eigene Farbe aus, für die Dimension von SCP-4837!"),
                new YesNoButton(416, "Soll SCP-4837 Regenbogen-Farben nutzten?", "Ja", "Nein", OnAvoid4837Hint, false, "Dies überschreibt deine Eigende Farbe, falls es Aktiv ist!"),
                new Keybind(ExampleId.Scp4837InteractKey, "SCP-4837 Interaktion", OnScp4837KeyPress, KeyCode.E, hint: Scp4837InteractionHint, isGlobal: true),
            };

            return _settings.ToArray();
        }
        private void OnAvoid4837Hint(ReferenceHub hub, bool yesorNo, SSTwoButtonsSetting ssTwoButtonsSetting)
        {
            Player player = Player.Get(hub);
            if (ssTwoButtonsSetting.SyncIsB)
            {
                player.ShowMeowHint("Feature Deaktiviert!");
            }
            if (ssTwoButtonsSetting.SyncIsA)
            {
                player.ShowMeowHint("Feature Aktiviert!");
            }
        }
        
        public List<KeyValuePair<Player, Color>> PlayerColorSelections = new List<KeyValuePair<Player, Color>>();
        
        private Dictionary<Player, string> SavedPlayerColors = new();

        private void Chose4837Color(ReferenceHub hub, SSDropdownSetting dropdown, string answer)
        {
            Player player = Player.Get(hub);

            int selectedIndex = dropdown.SyncSelectionIndexRaw;

            Color selectedColor = _presets[selectedIndex].Color;

            string hexColor = ColorUtility.ToHtmlStringRGB(selectedColor);

            string colorName = _presets[selectedIndex].Name;

            var existingSelection = PlayerColorSelections.FirstOrDefault(p => p.Key == player);

            if (existingSelection.Key != null)
            {
                PlayerColorSelections.Remove(existingSelection);
            }

            PlayerColorSelections.Add(new KeyValuePair<Player, Color>(player, selectedColor));
            
            if (SavedPlayerColors.ContainsKey(player))
            {
                SavedPlayerColors[player] = hexColor;
            }
            else
            {
                SavedPlayerColors.Add(player, hexColor);
            }

            player.ShowMeowHint($"Du hast die Farbe <color=#{hexColor}>{colorName}</color> ausgewählt!");
        }

        public Color GetPlayerColor(Player player)
        {
            if (SavedPlayerColors.TryGetValue(player, out var hexColor) && ColorUtility.TryParseHtmlString($"{hexColor}", out var color))
            {
                return color;
            }
            
            var selection = PlayerColorSelections.FirstOrDefault(p => p.Key == player);
            return selection.Equals(default(KeyValuePair<Player, Color>)) ? Color.white : selection.Value;
        }

        private void OnScp4837KeyPress(ReferenceHub hub, bool isPressed)
        {
            if (!isPressed)
                return;

            Player player = Player.Get(hub);
            if (player == null)
                return;
            StartTrading4837(player);
        }

        public override void OnRegistered()
        {
            Log.Info("SCP-4837 Interaction Menu has been registered.");
        }

        public override bool CheckAccess(ReferenceHub hub) => true;

        public override string Name { get; set; } = "Raven's garden Interaktion";
        public override int Id { get; set; } = -5151;
        public override Type MenuRelated { get; set; } = null;

        private static class ExampleId
        {
            public static readonly int Scp4837InteractKey = 1132;
        }

        public void StartTrading4837(Player player)
        {

            if (!PluginAPI.Core.Round.IsRoundStarted) return;

            if (_lastHintTimes.TryGetValue(player, out float lastHintTime) && Time.time - lastHintTime < 1f)
            {
                return;
            }

            _lastHintTimes[player] = Time.time;

            if (!Physics.Raycast(
                    new Ray(
                        player.ReferenceHub.PlayerCameraReference.position + player.GameObject.transform.forward * 0.3f,
                        player.ReferenceHub.PlayerCameraReference.forward), out RaycastHit raycastHit, 5)
                || raycastHit.collider.GetComponentInParent<SchematicObject>() is not { } schematicObject
                || schematicObject.Name != Plugin.Singleton.Main4837.SCP4837.Name)
            {
                return;
            }

            if (Plugin.Singleton.Main4837._Cooldown)
            {
                player.ShowMeowHint("<color=yellow>⚠️</color> <b><color=#757935>SCP-4837</b></color> hat Cooldown!");
                return;
            }

            if (Vector3.Distance(player.Position, Plugin.Singleton.Main4837.SCP4837.Position) > 4)
            {
                player.ShowMeowHint(
                    "<color=yellow>⚠️</color> Du bist zu weit von <b><color=#757935>SCP-4837</b></color> entfernt!");
                return;
            }

            if (player.Role.Type == RoleTypeId.Tutorial)
            {
                Plugin.Singleton.Main4837.Trade4837(player);
                return;
            }

            if (player.CurrentItem == null)
            {
                player.ShowMeowHint(
                    "<color=yellow>⚠️</color> Du musst <b>Brot</b> in der Hand halten, um mit <b><color=#757935>SCP-4837</b></color> zu handeln!");
                return;
            }

            if (CustomItem.TryGet(player.CurrentItem, out CustomItem customItem) && customItem?.Id == Plugin.Singleton.brot.Id)
            {
                Plugin.Singleton.Main4837.Trade4837(player);
                player.CurrentItem.Destroy();
            }

            else
            {
                player.ShowMeowHint(
                    "<color=yellow>⚠️</color> Du musst <b>Brot</b> in der Hand halten, um mit <b><color=#757935>SCP-4837</b></color> zu handeln!");
            }
        }
    }
}
