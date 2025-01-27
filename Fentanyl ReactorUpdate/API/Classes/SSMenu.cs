using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using InventorySystem.Items;
using MapEditorReborn.API.Features.Objects;
using SSMenuSystem.Features;
using SSMenuSystem.Features.Wrappers;
using UnityEngine;
using UserSettings.ServerSpecific;
using MEC;
using PlayerRoles;
using SSMenuSystem.Features;
using SSMenuSystem.Features.Wrappers;
using UnifiedEconomy.Helpers.Extension;
using UserSettings.ServerSpecific.Examples;

namespace Fentanyl_ReactorUpdate.API.SCP4837
{
    public class SCP4837InteractionMenu : Menu
    {
        private static readonly Translation Translation = Plugin.Singleton.Translation;
        private const string Scp4837InteractionHint = "Custom Interaktionen auf unserem Server wie z. B SCP-4837.";
        private List<ServerSpecificSettingBase> _settings;
        private SSTextArea _GiveHint;
        private SSTextArea _GiveHintCraft;
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
            _GiveHintCraft ??= new SSTextArea(591, "Keine Einstellung ausgewählt!");
            _GiveHint ??= new SSTextArea(411, "Keine Einstellung ausgewählt!");
            _presets ??= Plugin.Singleton.Translation.Presets.Select(p => new Preset(p.Key, p.Value)).ToList();

            _settings = new List<ServerSpecificSettingBase>
            {
                new YesNoButton(412, Translation.Scp4837HintDisplay, "Ja", "Nein", OnAvoid4837Hint, false),
                new YesNoButton(413, Translation.Scp4837CustomSounds, "Ja", "Nein", OnAvoid4837Hint, false, Translation.Scp4837CustomSoundsDescription),
                new YesNoButton(414, Translation.Scp4837JumpScareSounds, "Ja", "Nein", OnAvoid4837Hint, false),
                new Dropdown(415, Translation.Scp4837PocketDimensionColor, _presets.Select(x => x.Name).ToArray(), Chose4837Color, 1, SSDropdownSetting.DropdownEntryType.Hybrid, Translation.Scp4837PocketDimensionColorDescription),
                new YesNoButton(416, Translation.Scp4837RainbowColors, "Ja", "Nein", OnAvoid4837Hint, false, Translation.Scp4837RainbowColorsDescription),
                new YesNoButton(417, Translation.Scp4837CustomInteractionKey, "Ja", "Nein", OnAvoid4837Hint, false, Translation.Scp4837CustomInteractionKeyDescription),
                new Keybind(ExampleId.Scp4837InteractKey, Translation.Scp4837CustomInteraction, OnScp4837KeyPress, KeyCode.E, hint: Scp4837InteractionHint, isGlobal: true), 
                _GiveHint,
                new SSGroupHeader(Translation.SSHeaderCraftSystem),
                new SSTextArea(589, Translation.CraftingSystemDescription, SSTextArea.FoldoutMode.CollapseOnEntry),
                new Dropdown(590, Translation.CraftingSystemListRecipies, Plugin.Singleton.Config.Recipes.Select(x => x.Key).ToArray(), ChoseCrafting, 1, SSDropdownSetting.DropdownEntryType.Hybrid, Translation.CraftingSystemListRecipiesHnt),
                _GiveHintCraft,
                new SSGroupHeader(Translation.ServerDescriptionHeader),
                new SSTextArea(188, Translation.RavensReactorDescription, SSTextArea.FoldoutMode.CollapseOnEntry),
                new SSTextArea(788, Translation.RavensReactorDetails, SSTextArea.FoldoutMode.CollapseOnEntry),
                new SSTextArea(1788, Translation.DuckUnitDescription, SSTextArea.FoldoutMode.CollapseOnEntry),
                new SSTextArea(388, Translation.Scp4837Description, SSTextArea.FoldoutMode.CollapseOnEntry),
                new SSTextArea(588, Translation.Scp1356Description, SSTextArea.FoldoutMode.CollapseOnEntry),
            };
            return _settings.ToArray();
        }
        private void OnAvoid4837Hint(ReferenceHub hub, bool yesorNo, SSTwoButtonsSetting ssTwoButtonsSetting)
        {
            Player player = Player.Get(hub);
            if (ssTwoButtonsSetting.SyncIsB)
            {
                _GiveHint.SendTextUpdate("Feature Deaktiviert!");
            }
            if (ssTwoButtonsSetting.SyncIsA)
            {
                _GiveHint.SendTextUpdate("Feature Aktiviert!");
            }
        }
        
        public List<KeyValuePair<Player, Color>> PlayerColorSelections = new List<KeyValuePair<Player, Color>>();
        
        private Dictionary<Player, string> SavedPlayerColors = new();

private void ChoseCrafting(ReferenceHub hub, string answer, int rahhh, SSDropdownSetting dropdown)
{
    Player player = Player.Get(hub);
    if (player == null)
        return;

    if (!Plugin.Singleton.Config.Recipes.ContainsKey(answer))
    {
        _GiveHintCraft.SendTextUpdate("Invalid recipe selected!");
        return;
    }

    string recipeOutput = Plugin.Singleton.Config.Recipes[answer];
    string input1 = string.Empty;
    string input2 = string.Empty;

    foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
    {
        if (answer.StartsWith(item.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            input1 = item.ToString();
            input2 = answer.Substring(item.ToString().Length);
            break;
        }
    }

    if (!Enum.TryParse(input2, true, out ItemType validInput2))
    {
        _GiveHintCraft.SendTextUpdate("Invalid inputs for the selected recipe!");
        return;
    }

    string input1Translation = Plugin.Singleton.Enmm.ItemTranslations.ContainsKey((ItemType)Enum.Parse(typeof(ItemType), input1)) 
                                ? Plugin.Singleton.Enmm.ItemTranslations[(ItemType)Enum.Parse(typeof(ItemType), input1)] 
                                : "Unbekannt";
    string input2Translation = Plugin.Singleton.Enmm.ItemTranslations.ContainsKey(validInput2) 
                                ? Plugin.Singleton.Enmm.ItemTranslations[validInput2] 
                                : "Unbekannt";
    string outputTranslation = Plugin.Singleton.Enmm.ItemTranslations.ContainsKey((ItemType)Enum.Parse(typeof(ItemType), recipeOutput)) 
                                ? Plugin.Singleton.Enmm.ItemTranslations[(ItemType)Enum.Parse(typeof(ItemType), recipeOutput)] 
                                : "Custom Event";

    _GiveHintCraft.SendTextUpdate($"<color=yellow>Rezept Ausgewählt:</color>\n" +
                                  $"<color=green>Eingabe 1:</color> {input1Translation}\n" +
                                  $"<color=green>Eingabe 2:</color> {input2Translation}\n" +
                                  $"<color=blue>Ergebnis:</color> {outputTranslation}");
}

        
        private void Chose4837Color(ReferenceHub hub, string answer, int rahhh, SSDropdownSetting dropdown)
        {
            Player player = Player.Get(hub);
            if (player == null)
                return;

            int selectedIndex = dropdown.SyncSelectionIndexRaw;
            
            if (selectedIndex < 0 || selectedIndex >= _presets.Count)
                return;
            
            Color selectedColor = _presets[selectedIndex].Color;
            
            Plugin.Singleton.PlayerColorManager.SetPlayerColor(player, selectedColor, _GiveHint);
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
            Check1356Containment(player);
            StartTrading4837(player);
        }

        public override bool CheckAccess(ReferenceHub hub) => true;

        public override string Name { get; set; } = Translation.SSHeaderMain;
        public override int Id { get; set; } = -5151;
        public override Type MenuRelated { get; set; } = null;

        private static class ExampleId
        {
            public static readonly int Scp4837InteractKey = 1132;
        }

        public void Check1356Containment(Player Spieler)
        {
            if (!Physics.Raycast(
                    new Ray(
                        Spieler.ReferenceHub.PlayerCameraReference.position +
                        Spieler.GameObject.transform.forward * 0.3f,
                        Spieler.ReferenceHub.PlayerCameraReference.forward), out RaycastHit raycastHit, 5) ||
                raycastHit.collider.GetComponentInParent<SchematicObject>() is not { } schematicObject ||
                schematicObject.Name != Plugin.Singleton.RadiationDamage.SCP1356.Name)
            {
                return;
            }

            if (Vector3.Distance(Spieler.Position, Plugin.Singleton.RadiationDamage.SCP1356.Position) > 4)
            {
                Spieler.ShowMeowHint(Translation.SCP1356TooFarHint);
                return;
            }

            if (Spieler.Role.Type == RoleTypeId.Tutorial)
            {
                if (CustomItem.TryGet(Spieler.CurrentItem, out CustomItem custom) && custom?.Id == 1488)
                {
                    Spieler.ShowHint(Translation.SCP1356CapturedHint);
                    Spieler.AddBalance(50f);
                    string SCP1356RewardHint = Translation.SCP1356RewardHint.Replace("{0}", Spieler.GetPlayerFromDB().Balance.ToString());
                    Spieler.ShowMeowHintMoney(SCP1356RewardHint);
                    Server.ExecuteCommand("/forcesh");

                    foreach (Player player in Player.List.Where(p => p != Spieler))
                    {
                        player.ShowMeowHint(Translation.SCP1356DuckWaveHint);
                    }

                    Plugin.Singleton.SCP1356Breach = false;
                    Cassie.MessageTranslated(
                        Translation.SCP1356CassieMessage,
                        Translation.SCP1356CassieMessageTranslated
                    );

                    Plugin.Singleton.RadiationDamage.SCP1356.Destroy();
                    Plugin.Singleton.RadiationDamage.IsSCP1356Captured = true;
                }
                else
                {
                    Spieler.ShowHint(Translation.SCP1356DeviceRequiredHint);
                }
            }
            else
            {
                Spieler.ShowMeowHint(Translation.SCP1356InvalidRoleHint);
            }
            return;
        }

        public void StartTrading4837(Player player)
        {

            if (!PluginAPI.Core.Round.IsRoundStarted) return;

            if (_lastHintTimes.TryGetValue(player, out float lastHintTime) && Time.time - lastHintTime < 1f) return;

            _lastHintTimes[player] = Time.time;

            if (!Physics.Raycast(
                    new Ray(
                        player.ReferenceHub.PlayerCameraReference.position + player.GameObject.transform.forward * 0.3f,
                        player.ReferenceHub.PlayerCameraReference.forward), 
                    out RaycastHit hit, 
                    5) ||
                hit.collider.GetComponentInParent<SchematicObject>() is not { } schematic ||
                schematic.Name != Plugin.Singleton.Main4837.SCP4837.Name)
            {
                return;
            }

            if (Plugin.Singleton.Main4837.PlayerTradeCounts.TryGetValue(player, out int trades) && trades >= 3)
            {
                player.ShowMeowHint(Translation.SCP4837MaxTrades);
                return;
            }

            if (Plugin.Singleton.Main4837._Cooldown)
            {
                player.ShowMeowHint(Translation.SCP4837CooldownHint);
                return;
            }

            if (Vector3.Distance(player.Position, Plugin.Singleton.Main4837.SCP4837.Position) > 4)
            {
                player.ShowMeowHint(Translation.SCP4837TooFarHint);
                return;
            }

            if (player.Role.Type == RoleTypeId.Tutorial)
            {
                Plugin.Singleton.Main4837.Trade4837(player);
                return;
            }

            if (player.CurrentItem == null)
            {
                player.ShowMeowHint(Translation.SCP4837ItemRequiredHint);
                return;
            }

            if (CustomItem.TryGet(player.CurrentItem, out CustomItem customItem) && customItem?.Id == Plugin.Singleton.brot.Id)
            {
                Plugin.Singleton.Main4837.Trade4837(player);
                player.CurrentItem.Destroy();
            }
            else
            {
                player.ShowMeowHint(Translation.SCP4837ItemRequiredHint);
            }
        }
    }
}
