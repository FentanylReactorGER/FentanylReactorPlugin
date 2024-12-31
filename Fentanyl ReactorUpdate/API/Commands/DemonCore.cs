using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AdminToys;
using MapEditorReborn.API.Extensions;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using Fentanyl_ReactorUpdate.API.Extensions;
using Light = Exiled.API.Features.Toys.Light;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.Events.EventArgs;
using MapGeneration.Distributors;
using MEC;
using PlayerRoles;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Locker = Exiled.API.Features.Lockers.Locker;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class KillAreaCommand : ICommand
{
    public void SubEvents()
    {
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.InteractingLocker += OnLockerInteracting;
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted += OnButtonInteracted;
        Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
    }

    public void UnSubEvents()
    {
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.InteractingLocker -= OnLockerInteracting;
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted -= OnButtonInteracted;
        Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
    }
    public PrimitiveObject Demonarea { get; private set; }
    public bool DemonCorePedestal { get; set; } = true;
    public CancellationTokenSource KillArea;
    public Vector3 DemonCorePos;
    private float currentRadius;
    private float DemonCoreStartCooldown = Plugin.Singleton.Config.DemonCoreCooldown;
    private float damage = Plugin.Singleton.Config.KillAreaDamage;
    private EffectType effectone = Plugin.Singleton.Config.EffectOne;
    private EffectType effecttwo = Plugin.Singleton.Config.EffectTwo;
    private EffectType effectthree = Plugin.Singleton.Config.EffectThree;
    private int effectDuration= Plugin.Singleton.Config.EffectDuration;
    private CoroutineHandle coroutineHandle;
    
    public SchematicObject DemonCore { get; private set; }
    public Light DemonLight { get; private set; }
    
    string schemeDemonCore = "demon_core";
    private Speaker speaker;
    
        public string Command => "ForceDemonCore";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Spawns the Demon Core.";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.IsStarted)
            {
                response = "The Round is not Started!";
                return false;
            }
            Plugin.Singleton.KillAreaCommand.AddDemonCore();
            Plugin.Singleton.KillAreaCommand.StartCooldown();
            response = $"Demon Core spawned.";
            return true;
        }

        private void OnDetonated()
        {
            Plugin.Singleton.KillAreaCommand.AddDemonCore();
            Plugin.Singleton.KillAreaCommand.StartCooldown();
        }
        
        private void StartKillArea(float x, float y, float z)
        {
            Timing.RunCoroutine(KillDouble());
            DemonCorePos = new Vector3(x, y, z);
            AudioMeltdown();
            KillArea = new CancellationTokenSource();
            coroutineHandle = Timing.RunCoroutine(KillAreaCoroutine(KillArea.Token));  // Store the CoroutineHandle
            foreach (Room room in Room.List)
            {
                room.Color = Plugin.Singleton.Config.DemonCoreColorMelt;
            }
            Timing.CallDelayed(10f,
                () =>
                {
                    StartDamageIncrease();
                });
            Timing.CallDelayed(88f,
                () =>
                {   
                    MASSIVERadius();
                }); 
        }

        public void OnRoundStarted()
        {
            DemonCorePedestal = true;
            DemonCoreStartCooldown = Plugin.Singleton.Config.DemonCoreCooldown;
        }
        
        private void OnButtonInteracted(ButtonInteractedEventArgs ev)
        {
            if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonDeomCoreName)
            {
                ev.Button.Base.enabled = true;
                DemonCoreLight(10f, 10f, ev.Button.Position.x, ev.Button.Position.y, ev.Button.Position.z);
                StartKillArea(ev.Button.Position.x, ev.Button.Position.y, ev.Button.Position.z);
                ev.Button.Destroy();
            }
        }

        private bool DemonCoreUsed = false; 

        private void OnLockerInteracting(Exiled.Events.EventArgs.Player.InteractingLockerEventArgs ev)
        {
            if (ev.InteractingLocker.Base.gameObject.name == "Pedestal_DemonCore")
            {
                if (DemonCoreUsed)
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowMeowHint(Plugin.Singleton.Translation.DemonCoreAlrOpenHint);
                    return;
                }
                
                ev.InteractingChamber.RequiredPermissions = KeycardPermissions.ContainmentLevelThree | KeycardPermissions.ArmoryLevelThree | KeycardPermissions.ScpOverride;
                
                if (DemonCoreStartCooldown == 0)
                {
                    ev.InteractingChamber.RequiredPermissions = KeycardPermissions.ContainmentLevelThree | KeycardPermissions.ArmoryLevelThree | KeycardPermissions.ScpOverride;
                    ev.InteractingChamber.IsOpen = true;
                    ev.IsAllowed = false;
                    ev.InteractingChamber.Base.enabled = false;
                    ev.InteractingLocker.Base.enabled = false;
                    foreach (Player player in Player.List)
                    {
                        player.ShowMeowHint(Plugin.Singleton.Translation.DemonCoreOpenHint);
                    }
                    
                    DemonCoreUsed = true;

                    return;
                }
                
                ev.IsAllowed = false;
                ev.InteractingChamber.Base.enabled = false;
                ev.InteractingLocker.Base.enabled = false;

                ev.Player.ShowMeowHint(Plugin.Singleton.Translation.DemonCoreCooldownHint.Replace("{DemonCoreStartCooldown}", DemonCoreStartCooldown.ToString()));
                return;
            }
        }
    
        public void StartCooldown()
        {
            Timing.RunCoroutine(CheckPedestal());
        }
        
        private IEnumerator<float> CheckPedestal()
        {
            if (Round.IsEnded)
            {
                yield break;
            }

            DemonCoreStartCooldown = Plugin.Singleton.Config.DemonCoreCooldown;
            bool hasShownHint = false; 

            for (float i = DemonCoreStartCooldown; i >= 0; i--)
            {
                if (!hasShownHint && i == 1)
                {
                    foreach (Player player in Player.List)
                    {
                        player.ShowMeowHint(Plugin.Singleton.Translation.DemonCoreReadyToOpenHint);
                    }
                    hasShownHint = true; 
                }

                DemonCoreStartCooldown = i;
                yield return Timing.WaitForSeconds(1f);
            }
        }
        
        public void AudioMeltdown()
        {
            AudioPlayer globalPlayer = AudioPlayer.CreateOrGet($"GlobalAudioPlayerRad", onIntialCreation: (player) =>
            {
                speaker = player.AddSpeaker("Fentanyl Reactor Demon Core", DemonCorePos, 1f, false, 1f, 5000f);
            });
            foreach (var speaker in globalPlayer.SpeakersByName)
                speaker.Value.Position = DemonCorePos;
            globalPlayer.AddClip("Fentanyl Reactor Demon Core", 1f, false, true);
            if (Round.IsEnded)
            {
                globalPlayer.RemoveAllClips();
                globalPlayer.Destroy();
                globalPlayer.RemoveSpeaker("Fentanyl Reactor Demon Core");
            }
        }

        private IEnumerator<float> KillAreaCoroutine(CancellationToken token)
        {
            float expansionRate = Plugin.Singleton.Config.KillAreaExpansionRate;
            float checkInterval = Plugin.Singleton.Config.CheckInterval;

            while (Round.IsStarted && !Round.IsEnded)
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }
                currentRadius += expansionRate * checkInterval;

                foreach (Player player in Player.List)
                {
                    if (player.Role.Type == RoleTypeId.Spectator || player.Role.Type == RoleTypeId.Overwatch || player.Role.Type == RoleTypeId.Filmmaker)
                    {
                        yield return Timing.WaitForSeconds(checkInterval);
                    }

                    Vector3 playerPosition = player.Position;

                    if (Vector3.Distance(DemonCorePos, playerPosition) <= currentRadius)
                    {
                        player.EnableEffect(effectone, effectDuration);
                        player.EnableEffect(effecttwo, effectDuration);
                        player.EnableEffect(effectthree, effectDuration);
                        if (player.Role.Team != Team.Dead)
                        {
                            float healthToRemove = player.MaxHealth * (damage / 100f);
                            player.Hurt(healthToRemove, Plugin.Singleton.Translation.KillareaDeathReason);
                        }
                    }
                }

                yield return Timing.WaitForSeconds(checkInterval);
            }
        }

        public void MASSIVERadius()
        {
            currentRadius *= 15;
        }
        
        private bool stopCoroutine = false;  

        private IEnumerator<float> KillDouble()
        {
            float startDamage = 2f;
            float endDamage = 8f;
            float durationPhase1 = 68f;
            float startTime = Time.time;
            
            while (Time.time - startTime < durationPhase1 && !stopCoroutine)
            {
                float elapsedTime = Time.time - startTime;
                
                float lightRange = Mathf.Lerp(5f, 15f, elapsedTime / durationPhase1);
                float lightIntensity = Mathf.Lerp(1f, 10f, elapsedTime / durationPhase1); 

                UpdateLight(lightRange, lightIntensity, Plugin.Singleton.Config.DemonCoreColor);

                damage = Mathf.Lerp(startDamage, endDamage, elapsedTime / durationPhase1);
                yield return Timing.WaitForSeconds(1f);
            }

            damage = endDamage;
            
            startDamage = 10f;
            endDamage = 30f;
            startTime = Time.time;
            float phase2Duration = 20f;

            while (Time.time - startTime < phase2Duration && !stopCoroutine)
            {
                float elapsedTime = Time.time - startTime;
                
                float lightRange = Mathf.Lerp(15f, 30f, elapsedTime / phase2Duration); 
                float lightIntensity = Mathf.Lerp(10f, 20f, elapsedTime / phase2Duration); 

                UpdateLight(lightRange, lightIntensity, Plugin.Singleton.Config.DemonCoreColor);

                damage = Mathf.Lerp(startDamage, endDamage, elapsedTime / phase2Duration);
                yield return Timing.WaitForSeconds(1f);
            }

            damage = endDamage;
        }

        public void StartDamageIncrease()
        {
            stopCoroutine = false;
            coroutineHandle = Timing.RunCoroutine(KillDouble());  
        }

        public void StopDamageIncrease()
        {
            stopCoroutine = true;
        }

        private void DemonCoreLight(float Range, float Intensity, float x, float y, float z)
        {
            DemonLight = Light.Create();
            Vector3 LSourcePos = new Vector3(x, y, z);
            DemonLight.Position = LSourcePos;
            DemonLight.Range = Range;
            DemonLight.Intensity = Intensity;
            DemonLight.Color = Plugin.Singleton.Config.DemonCoreColor;
            DemonLight.Base.NetworkShadowType = LightShadows.None;
        }

        private void UpdateLight(float Range, float Intensity, Color Color)
        {
            DemonLight.Range = Range;
            DemonLight.Intensity = Intensity;
            DemonLight.Color = Color;
        }
        
        public void AddDemonCore()
        {
            foreach (Player player in Player.List.Where(p => Vector3.Distance(p.Position, new Vector3(25, 992, -27)) <= 1.5f))
            {
                player.Position = new Vector3(29.6f, 992, -28.8f);
            }
            StartCooldown();
            string schemeDemonCore = Plugin.Singleton.Config.DemonCoreSchemeName;
            Vector3 demoCorePos = Plugin.Singleton.Config.DemonCorePos;
            Vector3 demoCoreRot = Plugin.Singleton.Config.DemonCoreRot;
            Vector3 demonCoreScale = Plugin.Singleton.Config.DemonCoreScale;
            DemonCore = ObjectSpawner.SpawnSchematic(
                schemeDemonCore,
                demoCorePos,
                Quaternion.Euler(demoCoreRot),
                demonCoreScale,
                null
            );
        }
    
        private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            DemonCorePedestal = true;
            StopDamageIncrease();
            KillArea?.Cancel();
            KillArea = null;
            
            foreach (Room room in Room.List)
            {
                room.Color = new Color(1, 1, 1);
            }
            
            if (coroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
        }
    }