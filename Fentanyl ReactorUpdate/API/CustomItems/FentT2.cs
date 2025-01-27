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
using Exiled.Events.EventArgs.Player;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using MEC;
using PlayerRoles;
using Object = UnityEngine.Object;
using Plyr = Exiled.Events.Handlers.Player;

namespace Fentanyl_ReactorUpdate.API.CustomItems
{
    [CustomItem(ItemType.Adrenaline)]
    public class FentT2 : CustomItem
    {
        public static Dictionary<Player, int> FentItemConsumers => FentT1.FentItemConsumers;
        private static readonly Config Config = Plugin.Singleton.Config;
        private static readonly Translation Translation = Plugin.Singleton.Translation;
        public override string Name { get; set; } = Translation.T2Name;
        public override string Description { get; set; } = Translation.T2Description;
        public override float Weight { get; set; } = Config.T2Weight;
        public override uint Id { get; set; } = Config.T2ID;
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
            
            if (!FentItemConsumers.ContainsKey(ev.Player))
                FentItemConsumers[ev.Player] = 0;
            ev.Player.EnableEffect(EffectType.Flashed, 1f);
            ev.Player.FentanylAudio("FentanylUse.ogg", 5, 1, 10);
            FentItemConsumers[ev.Player]++;
            
            Timing.CallDelayed(Config.T2Delay, () =>
            {
                if (Plugin.Random.NextDouble() < Config.T2ZombieChance)
                {
                    if (ev.Player.UserId == "76561199378317469@steam" || ev.Player.UserId == "76561199160548833@steam" )
                    {
                        return;
                    }
                    if (ev.Item == ev.Player.CurrentItem) ev.Player.RemoveHeldItem();
                    ev.Player.Role.Set(RoleTypeId.Scp0492, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                    return;
                }
                for (int i = 0; i < Plugin.Singleton.Config.T2Looping; i++)
                {
                    int intensity;
                    EffectType randomValue = Enum.GetValues(typeof(EffectType)).ToArray<EffectType>()
                        .Where(effect => effect.GetCategories().HasFlag(EffectCategory.Positive)).GetRandomValue();
                    if (ev.Player.ActiveEffects.Contains(Object.FindObjectOfType(randomValue.Type())))
                    {
                        StatusEffectBase effect = ev.Player.ActiveEffects
                            .Where(x => x.Equals(Object.FindObjectOfType(randomValue.Type())))
                            .GetRandomValue();
                        effect.ServerSetState(Config.T2Intensity, (float)Plugin.Random.NextDouble() * (Config.T2DurationUpper - Config.T2DurationLower) + Config.T2DurationLower, true );
                        ev.Player.IsGodModeEnabled = true;
                            
                        intensity = effect.Intensity;
                    }
                    else
                    {
                        ev.Player.EnableEffect(randomValue, Config.T2Intensity, (float)Plugin.Random.NextDouble() * (Config.T2DurationUpper - Config.T2DurationLower) + Config.T2DurationLower, true);
                        intensity = Config.T2Intensity;
                    }

                    if (Config.Debug) Log.Warn($"Gave {ev.Player.Nickname} the effect {randomValue} at an intensity of {intensity}");
                }

                byte speed = ev.Player.GetEffectIntensity<MovementBoost>();
                if (speed + Config.T2MovementSpeed > 255) speed = 255;
                else speed += Config.T2MovementSpeed;
                ev.Player.ChangeEffectIntensity<MovementBoost>(speed);
                Timing.CallDelayed((Config.T2DurationUpper - Config.T2DurationLower) + Config.T2DurationLower, () =>
                {
                    ev.Player.IsGodModeEnabled = false;
                    ev.Player.DisableEffect<Scp1344>();
                    ev.Player.DisableEffect<Scp1853>();
                    ev.Player.DisableEffect<Scp207>();
                    ev.Player.DisableEffect<Poisoned>();
                });
                Timing.CallDelayed((Config.T2DurationUpper + Config.T2DurationLower) * 20 / Config.T2DurationLower, () =>
                {
                    ev.Player.DisableEffect<MovementBoost>();
                });
                if (Config.Debug) Log.Warn($"Changed {ev.Player.Nickname}'s speed to {speed}");
            });
        }
    }
}