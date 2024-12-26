using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Fentanyl_ReactorUpdate.Configs;
using MEC;
using PlayerRoles;
using Object = UnityEngine.Object;
using Plyr = Exiled.Events.Handlers.Player;

namespace Fentanyl_ReactorUpdate.API.CustomItems
{
    [CustomItem(ItemType.Adrenaline)]
    public class FentT1 : CustomItem
    {
        private static readonly Config Config = Plugin.Singleton.Config;
        private static readonly Translation Translation = Plugin.Singleton.Translation;
        public override string Name { get; set; } = Translation.T1Name;
        public override string Description { get; set; } = Translation.T1Description;
        public override float Weight { get; set; } = Config.T1Weight;
        public override uint Id { get; set; } = Config.T1ID;
        public override SpawnProperties SpawnProperties { get; set; }
        protected override void SubscribeEvents()
        {
            Plyr.UsingItem += UsingItem;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            
            Plyr.UsingItem -= UsingItem;
            base.UnsubscribeEvents();
        }

        private void UsingItem(UsingItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;
            Timing.CallDelayed(Config.T1Delay, () =>
            {
                if (Plugin.Random.NextDouble() < Config.T1ZombieChance)
                {
                    if (ev.Item == ev.Player.CurrentItem) ev.Player.RemoveHeldItem();
                    ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                    return;
                }
                for (int i = 0; i < Plugin.Singleton.Config.T1Looping; i++)
                {
                    int intensity;
                    EffectType randomValue = Enum.GetValues(typeof(EffectType)).ToArray<EffectType>()
                        .Where(effect => effect.GetCategories().HasFlag(EffectCategory.Positive)).GetRandomValue();
                    if (ev.Player.ActiveEffects.Contains(Object.FindObjectOfType(randomValue.Type())))
                    {
                        StatusEffectBase effect = ev.Player.ActiveEffects
                            .Where(x => x.Equals(Object.FindObjectOfType(randomValue.Type())))
                            .GetRandomValue();
                        effect.Intensity += Config.T1Intensity;
                        effect.ServerSetState(Config.T2Intensity, (float)Plugin.Random.NextDouble() * (Config.T2DurationUpper - Config.T2DurationLower) + Config.T2DurationLower, true );
                        intensity = effect.Intensity;
                    }
                    else
                    {
                        ev.Player.EnableEffect(randomValue, Config.T1Intensity, (float)Plugin.Random.NextDouble() * (Config.T1DurationUpper - Config.T1DurationLower) + Config.T1DurationLower, true);
                        intensity = Config.T1Intensity;
                    }

                    if (Config.Debug) Log.Warn($"Gave {ev.Player.Nickname} the effect {randomValue} at an intensity of {intensity}");
                }

                byte speed = ev.Player.GetEffectIntensity<MovementBoost>();
                if (speed + Config.T1MovementSpeed > 255) speed = 255;
                else speed += Config.T1MovementSpeed;
                ev.Player.ChangeEffectIntensity<MovementBoost>(speed);
                if (Config.Debug) Log.Warn($"Changed {ev.Player.Nickname}'s speed to {speed}");
                Timing.CallDelayed((Config.T1DurationUpper + Config.T1DurationLower) * 20 / Config.T1DurationLower, () =>
                {
                    ev.Player.DisableEffect<MovementBoost>();
                });
                if (Config.Debug) Log.Warn($"Changed {ev.Player.Nickname}'s speed to {speed}");
            });
        }
    }
}