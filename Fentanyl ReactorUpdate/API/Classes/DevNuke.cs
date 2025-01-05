using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events;
using Exiled.Events.Commands.PluginManager;
using Exiled.Events.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using Fentanyl_ReactorUpdate.API.Extensions;
using LightContainmentZoneDecontamination;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class DevNuke
    {
        // EXPERIMENTAL FEATURE!!!!
        public bool IsDevNuke { get; private set; } = false;
        private float DevNukeHintTimer { get; set; } = 92;
        private float DevNukeHintTimerNew { get; set; } = 0;
        private float DevNukeHintTimerBlastDoors { get; set; } = 10;
        private Color DevNukeColor = new Color(0.6f, 0.2f, 0.4f); 
        public void SubEvents()
        {
            //  Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
             //  Exiled.Events.Handlers.Player.Dying += OnDying;
            // Exiled.Events.Handlers.Player.Shooting += OnShooting;
            // Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            //Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            //Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            // Exiled.Events.Handlers.Player.Hurting += OnHurting;
            //Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += UsingMicroHIDEnergy;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }
        public void UnsubEvents()
        {
            //Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            //Exiled.Events.Handlers.Player.Dying -= OnDying;
            //Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            //Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            //Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            //Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            //Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            // Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= UsingMicroHIDEnergy;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            IsDevNuke = false;
            foreach (Player player in Player.List)
            {
                player.DisableEffect(EffectType.Ensnared);
                player.DisableEffect(EffectType.Invisible);
            }

            foreach (Door door in Door.List)
            {
                door.Base.enabled = true;
                door.Unlock();
            }
        }
        
        private void UsingMicroHIDEnergy(Exiled.Events.EventArgs.Player.UsingMicroHIDEnergyEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnDroppingItem(Exiled.Events.EventArgs.Player.DroppingItemEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnHurting(Exiled.Events.EventArgs.Player.HurtingEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnPickingUpItem(Exiled.Events.EventArgs.Player.PickingUpItemEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnEscaping(Exiled.Events.EventArgs.Player.EscapingEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnShooting(Exiled.Events.EventArgs.Player.ShootingEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnDying(Exiled.Events.EventArgs.Player.DyingEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }
        
        private void OnRespawningTeam(Exiled.Events.EventArgs.Server.RespawningTeamEventArgs ev)
        {
            ev.IsAllowed = false;
            if (IsDevNuke)
            {
                ev.IsAllowed = true;
            }
            if (Round.IsEnded)
            {
                ev.IsAllowed = true;
            }
        }

        public IEnumerator<float> BeforeBlastDoors()
        {
            while (DevNukeHintTimer > 0) 
            {
                foreach (Player player in Player.List)
                {
                    player.Broadcast(1, 
                        $"<color=red>DEV NUKE AKTIV</color>\n" +
                        $"<color=red>Server Neustart nach dieser Runde</color>\n" +
                        $"<color=purple>Verbleibende Zeit: {DevNukeHintTimer} Sekunden!</color>");
                }

                DevNukeHintTimer--; 
                yield return Timing.WaitForSeconds(1f); 
            }

            yield break;
        }

        public IEnumerator<float> AfterBlastDoors()
        {
            while (DevNukeHintTimerBlastDoors > 0) 
            {
                foreach (Player player in Player.List)
                {
                    player.Broadcast(1, 
                        $"<color=yellow>Explosionsschutztüren sind nun geschlossen</color>\n" +
                        $"<color=red>Verbleibende Zeit: {DevNukeHintTimerBlastDoors} Sekunden</color>");
                }

                DevNukeHintTimerBlastDoors--; 
                yield return Timing.WaitForSeconds(1f);
            }

            yield break;
        }

        
        public void StartDevNuke()
        {
            Plugin.Singleton.Reactor.RoomScheme.ChangeLight(DevNukeColor);
            foreach (Player player in Player.List)
            {
                player.EnableEffect(EffectType.Ensnared);
                player.EnableEffect(EffectType.Invisible);
                player.GlobalPlayer("Devnukee", "DEVNUKEEE", "DEVNUKE.OGG", 5000f);
                Cassie.Clear();
                Warhead.Stop();
                foreach (Door allDoors in Door.List)
                {
                    if (!allDoors.IsElevator)
                    {
                        allDoors.IsOpen = false; 
                        allDoors.Lock(DoorLockType.Warhead);
                    }
                }
                
                if (!Round.IsEnded)
                {
                    Timing.CallDelayed( 24f,
                        () =>
                        {
                            if (Round.IsEnded)
                            {
                                return;
                            }
                            player.DisableEffect(EffectType.Ensnared);
                            player.DisableEffect(EffectType.Invisible);
                            IsDevNuke = true;
                            foreach (Room room in Room.List)
                            {
                                room.Color = DevNukeColor;
                            }
                            Timing.RunCoroutine(BeforeBlastDoors());
                            foreach (Door checkpoint in Door.List)
                            {
                                if (!checkpoint.IsElevator) 
                                {
                                    checkpoint.IsOpen = true; 

                                    if (checkpoint.IsCheckpoint)
                                    {
                                        checkpoint.Base.enabled = false; 
                                    }
                                }
                            }
                            Timing.CallDelayed( 100f,
                                () =>
                                {
                                    Warhead.CloseBlastDoors();
                                    player.ClearBroadcasts();
                                    DevNukeHintTimer = 10;
                                    Timing.RunCoroutine(AfterBlastDoors());
                                    Timing.CallDelayed( 10f,
                                        () =>
                                        {
                                            Warhead.Detonate();
                                            Server.ExecuteCommand("/rnr");
                                        });   
                                });   
                        });   
                }
            }
        }
    }
}