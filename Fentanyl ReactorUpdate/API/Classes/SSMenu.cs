using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using Fentanyl_ReactorUpdate.API.Classes;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class SSMenu
    {
        private CancellationTokenSource _playerPosCts;
        private static float XPos { get; set; }
        private static float YPos { get; set; }
        private static float ZPos { get; set; }
        private Vector3 _roomPos;
        private readonly HashSet<Player> _playersInReactor = new();

        public SSMenu()
        {
            _roomPos = new Vector3(XPos, YPos, ZPos);
            SubEvents();
        }

        public void Destroy()
        {
            UnSubEvents();
        }

        private void SubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.Verified += OnPlayerVerified;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnButtonTriggered;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnDropdownTriggered;
            RegisterSSButton();
        }

        private void UnSubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.Verified -= OnPlayerVerified;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnButtonTriggered;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnDropdownTriggered;
            UnregisterSSButton();
        }

        private void OnPlayerVerified(VerifiedEventArgs ev)
        {
            ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
        }

        private void RegisterSSButton()
        {
            var admin = new HeaderSetting("Fentanyl Reaktor Admin");
            var testButton = new ButtonSetting(
                Plugin.Singleton.Config.ServerSpecificSettingId,
                Plugin.Singleton.Translation.SSSSLabelTp,
                Plugin.Singleton.Translation.SSSSTpButton,
                Plugin.Singleton.Config.ServerSpecificSettingHoldTime,
                Plugin.Singleton.Translation.SSSSDescTp);

            var player = new HeaderSetting(Plugin.Singleton.Translation.SSSSheaderPlayer);
            var fuelReactor = new ButtonSetting(
                Plugin.Singleton.Config.ServerSpecificSettingIdFuel,
                Plugin.Singleton.Translation.SSSSLabelFuel,
                Plugin.Singleton.Translation.SSSSFuelButton,
                Plugin.Singleton.Config.ServerSpecificSettingHoldTime,
                Plugin.Singleton.Translation.SSSSDescFuel);

            var startReactor = new DropdownSetting(
                Plugin.Singleton.Config.ServerSpecificSettingIdStart,
                Plugin.Singleton.Translation.SSSSStartName,
                new[] {
                    Plugin.Singleton.Translation.SSSSlStage1,
                    Plugin.Singleton.Translation.SSSSlStage2,
                    Plugin.Singleton.Translation.SSSSlStage3
                },
                1,
                SSDropdownSetting.DropdownEntryType.Regular,
                Plugin.Singleton.Translation.SSSSStartDesc);

            IEnumerable<SettingBase> settings = new SettingBase[]
            {
                admin,
                testButton,
                player,
                fuelReactor,
                startReactor
            };
            SettingBase.Register(settings);
            SettingBase.SendToAll();
        }

        private void UnregisterSSButton()
        {
            SettingBase.Unregister();
        }

        private void OnRoundStarted()
        {
            if (Plugin.Singleton.Config.EnterHint)
            {
                _playerPosCts = new CancellationTokenSource();
                Timing.RunCoroutine(PlayerPos(_playerPosCts.Token));
            }

            XPos = Plugin.Singleton.Reactor.RoomScheme.Position.x;
            YPos = Plugin.Singleton.Reactor.RoomScheme.Position.y + 3;
            ZPos = Plugin.Singleton.Reactor.RoomScheme.Position.z;
            _roomPos = new Vector3(XPos, YPos, ZPos);

            Log.Info("Round Started");
            Log.Info($"{Plugin.Singleton.Reactor.RoomScheme.Position} {_roomPos}");
        }

        private IEnumerator<float> PlayerPos(CancellationToken token)
        {
            while (Round.IsStarted && !Round.IsEnded)
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                foreach (var player in Player.List.Where(p => p.IsAlive))
                {
                    bool isInReactor = Vector3.Distance(player.Position, _roomPos) < 8;

                    if (isInReactor)
                    {
                        if (!_playersInReactor.Contains(player))
                        {
                            player.ShowMeowHint($"{Plugin.Singleton.Translation.EnterFentanylReactor.Replace("{PlayerName}", player.Nickname)}");
                            _playersInReactor.Add(player);
                        }
                    }
                    else
                    {
                        _playersInReactor.Remove(player);
                    }
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }

        private void OnButtonTriggered(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            if (!Player.TryGet(hub, out var player)) return;

            if (settingBase is SSButton fentTpButton && fentTpButton.SettingId == Plugin.Singleton.Config.ServerSpecificSettingId)
            {
                HandleTeleportButton(player);
            }
            else if (settingBase is SSButton fuelButton && fuelButton.SettingId == Plugin.Singleton.Config.ServerSpecificSettingIdFuel)
            {
                HandleFuelButton(player);
            }
        }

        private void HandleTeleportButton(Player player)
        {
            if (!Round.IsStarted)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSRoundNotStarted);
                return;
            }

            if (player.IsScp)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSPlayerIsSCP);
                return;
            }

            if (!player.CheckPermission(Plugin.Singleton.Translation.TeleportFentanylPremission))
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.TeleportFentanylNoPrem);
                return;
            }

            player.Position = _roomPos;
        }

        private void HandleFuelButton(Player player)
        {
            if (!Round.IsStarted)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSRoundNotStarted);
                return;
            }

            if (player.IsScp)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSPlayerIsSCP);
                return;
            }

            if (Vector3.Distance(player.Position, _roomPos) > 12 &&
                player.Position.y >= -1015 && player.Position.y <= -1005)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSStartNotInReactor);
                return;
            }

            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.FuelCommandName} {player.Id}");
        }

        private void OnDropdownTriggered(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            if (!Player.TryGet(hub, out var player)) return;

            if (settingBase is SSDropdownSetting startReactor && startReactor.SettingId == Plugin.Singleton.Config.ServerSpecificSettingIdStart)
            {
                HandleDropdownSelection(player, startReactor);
            }
        }

        private void HandleDropdownSelection(Player player, SSDropdownSetting startReactor)
        {
            if (!Round.IsStarted) return;

            if (player.IsScp)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSSPlayerIsSCP);
                return;
            }

            if (Vector3.Distance(player.Position, _roomPos) > 12)
            {
                player.ShowMeowHint(Plugin.Singleton.Translation.SSSStartNotInReactor);
                return;
            }

            string commandName = Plugin.Singleton.Translation.CommandName;
            switch (startReactor.SyncSelectionText)
            {
                case var text when text == Plugin.Singleton.Translation.SSSSlStage1:
                    Server.ExecuteCommand($"/{commandName} {player.Id} 1");
                    break;
                case var text when text == Plugin.Singleton.Translation.SSSSlStage2:
                    Server.ExecuteCommand($"/{commandName} {player.Id} 2");
                    break;
                case var text when text == Plugin.Singleton.Translation.SSSSlStage3:
                    Server.ExecuteCommand($"/{commandName} {player.Id} 3");
                    break;
            }
        }

        private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            _playerPosCts?.Cancel();
            _playerPosCts = null;
            _playersInReactor.Clear();
        }
    }
}
