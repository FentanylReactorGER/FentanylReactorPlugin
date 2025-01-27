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
    public class ShopMenu : Menu
    {
        private static readonly Translation Translation = Plugin.Singleton.Translation;
        private static readonly Config Config = Plugin.Singleton.Config;
        private List<ServerSpecificSettingBase> _settings;
        private List<Preset> _presets;
        
        private SSTextArea _checkKonto;
        
        private readonly Dictionary<Player, Preset> _playerSelectedItems = new();

        private List<RoomType> _presetRoomTypes = Config._presetRoomTypes;

        private List<RoomType> _presetRoomTypesNonNormal = Config._presetRoomTypesNonNormal;
        
        public class Preset
        {
            public string Name { get; }
            public ItemType? Item { get; }
            public uint? CustomItemId { get; }
            public float Price { get; }
            
            public Preset(string name, ItemType item, float price)
            {
                Name = name;
                Item = item;
                CustomItemId = null;
                Price = price;
            }
            
            public Preset(string name, uint customItemId, float price)
            {
                Name = name;
                Item = null;
                CustomItemId = customItemId;
                Price = price;
            }
        }

        public override ServerSpecificSettingBase[] Settings => GetSettings();

        private ServerSpecificSettingBase[] GetSettings()
        {
            _checkKonto ??= new SSTextArea(null, "Kein Kontostand");
            _presets ??= Plugin.Singleton.Config.presets.Select(p => 
            {
                if (p.Item.HasValue) 
                {
                    return new Preset(p.Name, p.Item.Value, p.Price);
                }
                else if (p.CustomItemId.HasValue)  
                {
                    return new Preset(p.Name, p.CustomItemId.Value, p.Price);
                }
                return null;  
            }).Where(p => p != null).ToList();
            
            _settings = new List<ServerSpecificSettingBase>
            {
                new Dropdown(415, Translation.ItemDropdownLabel, _presets.Select(x => x.Name).ToArray(), BuyItemChoosing, 1, SSDropdownSetting.DropdownEntryType.Hybrid),
                new Button(513, Translation.ConfirmPurchaseButtonLabel, Translation.ConfirmPurchaseButtonAction, Buy),
                new Button(514, Translation.CheckBalanceButtonLabel, Translation.CheckBalanceButtonAction, CheckPlayer),
                _checkKonto,
            };

            return _settings.ToArray();
        }

        private void CheckPlayer(ReferenceHub hub, SSButton ssTwoButtonsSetting)
        {
            Player sender = Player.Get(hub);
            float playerBalance = sender.GetPlayerFromDB().Balance;
            
            if (sender == null)
            {
                Log.Warn("Player konnte nicht gefunden werden.");
                return;
            }
            
            if (!playerBalance.Equals(0))
            {
                _checkKonto.SendTextUpdate($"Du hast {playerBalance} {Plugin.Singleton.WebSocketServer.GetCustomMessage(sender.UserId)}");
                Log.Info(sender.UserId);
            }
            else
            {
                _checkKonto.SendTextUpdate("Du hast kein Geld!");
            }
        }
        
        private void Buy(ReferenceHub hub, SSButton ssTwoButtonsSetting)
        {
            Player player = Player.Get(hub);

            if (player == null)
            {
                Log.Warn("Player konnte nicht gefunden werden.");
                return;
            }

            if (!Round.IsStarted)
            {
                _checkKonto.SendTextUpdate("Die Runde ist nicht Gestartet!");
                return;
            }

            if (player.Role.Team == Team.SCPs || (player.Role.Team == Team.Dead))
            {
                _checkKonto.SendTextUpdate("Du kannst den Shop mit deine Rolle nicht nutzten!");
                return;
            }
            if (player.Role.Type == RoleTypeId.FacilityGuard || player.Role.Type == RoleTypeId.Scientist ||
                player.Role.Type == RoleTypeId.ClassD)
            {
                if (!_presetRoomTypes.Contains(player.CurrentRoom.Type))
                {
                    _checkKonto.SendTextUpdate("Du kannst nur in Checkpoints den Shop nutzten!");
                    return;
                }
            }
            if (player.Role.Type != RoleTypeId.FacilityGuard || player.Role.Type != RoleTypeId.Scientist ||
                player.Role.Type != RoleTypeId.ClassD)
            {
                if (!_presetRoomTypesNonNormal.Contains(player.CurrentRoom.Type))
                {
                    _checkKonto.SendTextUpdate("Du kannst nur in Checkpoints oder der Surface den Shop nutzten!");
                    return;
                }
            }
            var elapsedTime = Round.ElapsedTime;
            if (player.Role.Type == RoleTypeId.FacilityGuard || player.Role.Type == RoleTypeId.Scientist ||
                player.Role.Type == RoleTypeId.ClassD)
            {
                if (!player.UserId.Contains("76561199160548833"))
                {
                    if (elapsedTime.TotalMinutes <= 4)
                    {
                        _checkKonto.SendTextUpdate($"Erst nach 5 Minuten kannst du den Shop nutzten! \n Rundenzeit: {Math.Round(elapsedTime.TotalMinutes)} Minuten.");
                        return;
                    }
                    else if (elapsedTime.TotalMinutes <= 5)
                    {
                        _checkKonto.SendTextUpdate($"Erst nach 5 Minuten kannst du den Shop nutzten! \n Rundenzeit: 4:{elapsedTime.Seconds} Minuten.");
                        return;
                    }
                }
            }
            if (!_playerSelectedItems.TryGetValue(player, out Preset selectedPreset))
            {
                _checkKonto.SendTextUpdate("Bitte wähle zuerst einen Gegenstand aus!");
                return;
            }

            if (player.IsCuffed)
            {
                _checkKonto.SendTextUpdate($"Du bist Gefesselt!");
                return;
            }

            if (player.Items.Count.Equals(8))
            {
                _checkKonto.SendTextUpdate($"Dein Inventar ist Voll!");
                return;
            }

            foreach (var item in player.Items)
            {
                if (item.Type == ItemType.ArmorCombat || item.Type == ItemType.ArmorLight ||
                    item.Type == ItemType.ArmorHeavy)
                {
                    if (selectedPreset.Item == ItemType.ArmorHeavy)
                    {
                        _checkKonto.SendTextUpdate($"Dein Inventar ist Voll!");
                        return;
                    }
                }
            }
            if (player.IsCuffed)
            {
                _checkKonto.SendTextUpdate($"Du bist Gefesselt!");
                return;
            }
            
            float playerBalance = player.GetPlayerFromDB().Balance;

            if (playerBalance < selectedPreset.Price)
            {
                _checkKonto.SendTextUpdate($"Du hast nicht genug Geld! \n Du benötigst {selectedPreset.Price} {Plugin.Singleton.WebSocketServer.GetCustomMessage(player.UserId)}!");
                return;
            }
            
            player.RemoveBalance(selectedPreset.Price);
            
            if (selectedPreset.Item.HasValue)
            {
                player.AddItem(selectedPreset.Item.Value);
            }
            else if (selectedPreset.CustomItemId.HasValue)
            {
                CustomItem.TryGive(player, selectedPreset.CustomItemId.Value);
            }
            else if (!selectedPreset.CustomItemId.HasValue && !selectedPreset.Item.HasValue)
            {
                Log.Error("Preset does not contain a valid item or custom item ID!");
            }


            _checkKonto.SendTextUpdate($"Du hast {selectedPreset.Name} für {selectedPreset.Price} {Plugin.Singleton.WebSocketServer.GetCustomMessage(player.UserId)} erfolgreich gekauft!");
            
            _playerSelectedItems.Remove(player);
        }

        private void BuyItemChoosing(ReferenceHub hub, string option, int optionNum, SSDropdownSetting dropdownSetting)
        {
            Player player = Player.Get(hub);

            if (player == null)
            {
                Log.Warn("Player konnte nicht gefunden werden.");
                return;
            }

            // Den ausgewählten Dropdown-Wert mit dem entsprechenden Preset verknüpfen
            Preset selectedPreset = _presets.FirstOrDefault(p => p.Name == option);

            if (selectedPreset == null)
            {
                Log.Warn($"Kein passendes Item für die Auswahl '{option}' gefunden.");
                return;
            }

            // Speichere die Auswahl des Spielers
            _playerSelectedItems[player] = selectedPreset;

            // Feedback geben
            _checkKonto.SendTextUpdate($"Du hast {selectedPreset.Name} ausgewählt. \n  Preis: {selectedPreset.Price} {Plugin.Singleton.WebSocketServer.GetCustomMessage(player.UserId)}.");
        }

        public override bool CheckAccess(ReferenceHub hub) => true;

        public override string Name { get; set; } = "Raven's Garden Shop Menü";
        public override int Id { get; set; } = -5152;
        public override Type MenuRelated { get; set; } = null;

        private static class ExampleId
        {
            public static readonly int Scp4837InteractKey = 1314;
        }
    }
}
