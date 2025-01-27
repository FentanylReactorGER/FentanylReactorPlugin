using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.Commands.UtilityCommands;
using MEC; 
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class AntiCamp
    {
        private readonly Dictionary<Player, RoomType> playerPositions = new();
        private readonly Dictionary<Player, float> playerTimers = new();
        private readonly List<Player> playersHint = new();
        private CoroutineHandle antiCampCoroutine;

        public void SubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += Starting;
            Exiled.Events.Handlers.Player.Destroying += OnPlayerDestroying;
        }

        public void UnsubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= Starting;
            Exiled.Events.Handlers.Player.Destroying -= OnPlayerDestroying;
            Timing.KillCoroutines(antiCampCoroutine);
        }

        private void Starting()
        {
            if (antiCampCoroutine.IsRunning)
                Timing.KillCoroutines(antiCampCoroutine);

            antiCampCoroutine = Timing.RunCoroutine(AntiCampChecker());
        }

        private void OnPlayerDestroying(Exiled.Events.EventArgs.Player.DestroyingEventArgs ev)
        {
            playerPositions.Remove(ev.Player);
            playerTimers.Remove(ev.Player);
        }

        private IEnumerator<float> AntiCampChecker()
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    if (!player.IsAlive) continue;

                    RoomType currentPosition = player.CurrentRoom.Type;

                    // Check if player is already in the dictionary
                    if (playerPositions.ContainsKey(player))
                    {
                        if (playerPositions[player] == currentPosition)
                        {
                            // Increment timer if position hasn't changed
                            playerTimers[player] += 1f; // 1 second per coroutine tick
                            if (playerTimers[player] >= 120f) // 120 seconds
                            {
                                if (Plugin.Singleton.Enmm.RoomTranslations.TryGetValue(player.CurrentRoom.Type, out string roomTranslation))
                                {
                                    foreach (Player pbroadcast in Player.List.Where(p => p != player))
                                    {
                                        if (!playersHint.Contains(pbroadcast))
                                        {
                                            pbroadcast.ShowMeowHintDur($"Der Spieler {player.Nickname} verweilt seit über 2 Minuten im Raum: {roomTranslation}", 15);
                                            playersHint.Add(pbroadcast);
                                        }
                                    }
                                    if (!playersHint.Contains(player))
                                    {
                                        player.ShowMeowHintDur($"{player.Nickname} du verweilst seit über 2 Minuten im Raum: {roomTranslation}! \n Deine Position wurde Preisgeben....", 15);
                                        playersHint.Add(player);
                                    }
                                }
                                else
                                {
                                    foreach (Player pbroadcast in Player.List.Where(p => p != player))
                                    {
                                        if (!playersHint.Contains(pbroadcast))
                                        {
                                            pbroadcast.ShowMeowHintDur($"Der Spieler {player.Nickname} verweilt seit über 2 Minuten im Raum: {player.CurrentRoom.Name}", 15);
                                            playersHint.Add(pbroadcast);
                                        }
                                    }
                                    if (!playersHint.Contains(player))
                                    {
                                        player.ShowMeowHintDur($"{player.Nickname} du verweilst seit über 2 Minuten im Raum: {player.CurrentRoom.Name}! \n Deine Position wurde Preisgeben....", 15);
                                        playersHint.Add(player);
                                    }
                                }
                                Log.Info($" {player.Nickname} has been camping in the same location for over 2 minutes!");
                            }
                        }
                        else
                        {
                            // Reset timer and update position if they moved
                            playersHint.Clear();
                            playerPositions[player] = currentPosition;
                            playerTimers[player] = 0f;
                        }
                    }
                    else
                    {
                        // Add new player to the dictionary
                        playersHint.Clear();
                        playerPositions[player] = currentPosition;
                        playerTimers[player] = 0f;
                    }
                }

                yield return Timing.WaitForSeconds(1f); // Check every second
            }
        }
    }
}
