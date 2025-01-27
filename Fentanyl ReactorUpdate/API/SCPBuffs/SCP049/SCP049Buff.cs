using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.SCPBuffs.SCP049;

public class SCP049Buff
{
    private int ZombieCount { get; set; }
    public void SubEvents()
    {
        Exiled.Events.Handlers.Scp049.FinishingRecall += GetsZombie;
        Exiled.Events.Handlers.Player.Died += LosesZombie;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.Spawned += SpawningEvent;
    }

    public void UnSubEvents()
    {
        Exiled.Events.Handlers.Scp049.FinishingRecall -= GetsZombie;
        Exiled.Events.Handlers.Player.Died -= LosesZombie;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.Spawned -= SpawningEvent;
    }

    private void SpawningEvent(SpawnedEventArgs ev)
    {
        if (ev.Player.Role.Type == RoleTypeId.Scp0492)
        {
            if (ZombieCount != 5)
            {
                ZombieCount++;
                Log.Info(ZombieCount);
            }
        }
        if (ev.Player.Role.Type == RoleTypeId.Scp049)
        {
            Timing.RunCoroutine(CheckDocAbilitys(ev.Player));
            Log.Info(ZombieCount);
        }
    }
    
    private void OnRoundStart()
    {
        ZombieCount = 0;
    }

    private IEnumerator<float> CheckDocAbilitys(Player player)
    {
        while (Round.IsStarted && player.Role.Type == RoleTypeId.Scp049)
        {
            if (ZombieCount == 0)
            {
                yield return Timing.WaitForSeconds(1f);
            }
            else if (ZombieCount == 1)
            {
                foreach (Player Zombie in Player.List.Where(p => p.IsAlive && Vector3.Distance(p.Position, player.Position) < 5 && p.Role.Type == RoleTypeId.Scp0492))
                {
                    if ((int)Zombie.Health != (int)Zombie.MaxHealth)
                    {
                        Zombie.Heal(3);
                    }
                    Zombie.EnableEffect(EffectType.MovementBoost, 5, 1);
                }
                yield return Timing.WaitForSeconds(1f);
            }
            else if (ZombieCount == 2)
            {
                foreach (Player Zombie in Player.List.Where(p => p.IsAlive && Vector3.Distance(p.Position, player.Position) < 15 && p.Role.Type == RoleTypeId.Scp0492))
                {
                    if ((int)Zombie.Health != (int)Zombie.MaxHealth)
                    {
                        Zombie.Heal(5);
                    }
                    Zombie.EnableEffect(EffectType.MovementBoost, 2, 1);
                }
                yield return Timing.WaitForSeconds(1f);
            }
            else if (ZombieCount == 3)
            {
                foreach (Player Zombie in Player.List.Where(p => p.IsAlive && Vector3.Distance(p.Position, player.Position) < 20 && p.Role.Type == RoleTypeId.Scp0492))
                {
                    if ((int)Zombie.Health != (int)Zombie.MaxHealth)
                    {
                        Zombie.Heal(10);
                    }

                    Zombie.EnableEffect(EffectType.MovementBoost, 5, 1);
                }
                yield return Timing.WaitForSeconds(1f);
            }
            else if (ZombieCount == 4)
            {
                foreach (Player Zombie in Player.List.Where(p => p.IsAlive && Vector3.Distance(p.Position, player.Position) < 25 && p.Role.Type == RoleTypeId.Scp0492))
                {
                    if ((int)Zombie.Health != (int)Zombie.MaxHealth)
                    {
                        Zombie.Heal(15);
                    }
                    Zombie.EnableEffect(EffectType.MovementBoost, 10, 1);
                }
                int nearbyZombieCount = Player.List.Count(p => p.IsAlive && p.Role.Type == RoleTypeId.Scp0492 && Vector3.Distance(player.Position, p.Position) < 17);
                
                if (nearbyZombieCount > 0)
                {
                    byte boostIntensity = (byte)(nearbyZombieCount * 5);
                    player.EnableEffect(EffectType.MovementBoost, boostIntensity, 1);
                }
                yield return Timing.WaitForSeconds(1f);
            }
            else if (ZombieCount > 5)
            {
                foreach (Player Zombie in Player.List.Where(p => p.IsAlive && Vector3.Distance(p.Position, player.Position) < 30 && p.Role.Type == RoleTypeId.Scp0492))
                {
                    if ((int)Zombie.Health != (int)Zombie.MaxHealth)
                    {
                        Zombie.Heal(25);
                    }
                    Zombie.EnableEffect(EffectType.MovementBoost, 10, 1);
                }
                int nearbyZombieCount = Player.List.Count(p => p.IsAlive && p.Role.Type == RoleTypeId.Scp0492 && Vector3.Distance(player.Position, p.Position) < 7);
                
                if (nearbyZombieCount > 0)
                {
                    byte boostIntensity = (byte)(nearbyZombieCount * 10);
                    player.EnableEffect(EffectType.MovementBoost, boostIntensity, 1);
                }
                yield return Timing.WaitForSeconds(1f);
            }
            yield return Timing.WaitForSeconds(1f);
        }
        yield break;
    }
    
    private void LosesZombie(DiedEventArgs ev)
    {
        if (ev.Player.Role.Type == RoleTypeId.Scp0492)
        {
            if (ZombieCount != 0)
            {
                ZombieCount--;
            }
        }
    }
    
    private void GetsZombie(FinishingRecallEventArgs ev)
    {
        if (ZombieCount != 5)
        {
            ZombieCount++;
        }
        Log.Info(ZombieCount);
        return;
    }
}