using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using PlayerRoles;
using SSMenuSystem.Features;
using SSMenuSystem.Features.Wrappers;
using UnifiedEconomy.Helpers.Extension;
using System.Data.SQLite;
using Fentanyl_ReactorUpdate.Configs;
using UserSettings.ServerSpecific;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class AdminMenu : Menu
    {
        private static readonly Translation Translation = Plugin.Singleton.Translation;
        private static readonly Config Config = Plugin.Singleton.Config;
        private List<ServerSpecificSettingBase> _settings;
        private List<Preset> _presets;

        private SSTextArea _Respone;

        public readonly Dictionary<string, uint> _playerSelectedItems = new()
        {
            { "RadioPainkillers", 1488 },
            { "bread", 1489 },
        };
        public readonly Dictionary<Player, uint> _playerSelectedCItems = new();
        
        public override ServerSpecificSettingBase[] Settings => GetSettings();

        private ServerSpecificSettingBase[] GetSettings()
        {
            _Respone ??= new SSTextArea(null, "Kein Kontostand");
            
            _settings = new List<ServerSpecificSettingBase>
            {
                new Dropdown(612, "Custom Items", _playerSelectedItems.Select(p => p.Key).ToArray(), ChooseCustomItem, 1, SSDropdownSetting.DropdownEntryType.Hybrid),
                new Button(613, Translation.ConfirmPurchaseButtonLabel, Translation.ConfirmPurchaseButtonAction, GiveCustomItem),
                _Respone,
            };

            return _settings.ToArray();
        }

        private void GiveCustomItem(ReferenceHub hub, SSButton ssTwoButtonsSetting)
        {
            Player player = Player.Get(hub);

            if (player == null || player.Role.Team == Team.Dead || player.Role.Team == Team.SCPs)
            {
                _Respone.SendTextUpdate("Du kannst in deinem Zustand keine Custom Items bekommen!");
                return;
            }

            if (!_playerSelectedCItems.ContainsKey(player))
            {
                _Respone.SendTextUpdate($"Du hast kein Item ausgewählt!");
                return; 
            }

            if (!_playerSelectedCItems.TryGetValue(player, out uint selected) || !CustomItem.TryGet(selected, out CustomItem customItem))
            {
                _Respone.SendTextUpdate("Fehler!");
                return; 
            }
            
            if (CustomItem.TryGive(player, selected))
            {
                _Respone.SendTextUpdate($"Custom Item {customItem!.Name} wird gegeben!");
            }
            else
            {
                _Respone.SendTextUpdate($"Fehler beim Geben von Custom Item {customItem!.Name}!");
            }
            
            _playerSelectedCItems.Remove(player);

        }
        
        private void ChooseCustomItem(ReferenceHub hub, string itemName, int number, SSDropdownSetting setting)
        {
            Player player = Player.Get(hub);

            if (player == null)
            {
                _Respone.SendTextUpdate("Du kannst in deinem Zustand keine Custom Items bekommen!");
                return;
            }

            if (CustomItem.TryGet(itemName, out CustomItem customItem))
            {
                if (!_playerSelectedCItems.ContainsKey(player) && !_playerSelectedCItems.ContainsValue(customItem!.Id))
                {
                    _playerSelectedCItems.Add(player, customItem.Id);
                    _Respone.SendTextUpdate($"Item {customItem.Name} ist ausgewählt!");
                }
            }
        }
        
        public override bool CheckAccess(ReferenceHub hub) => true;

        public override string Name { get; set; } = "Raven's Garden Adminpanel";
        public override int Id { get; set; } = -5153;
        public override Type MenuRelated { get; set; } = null;
    }
}
