using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace Fentanyl_ReactorUpdate.API.CustomItems
{
    public class CustomItemLight
    {
        private Dictionary<Pickup, Light> ActiveLights { get; set; } = new();
        private Dictionary<uint, Color> CustomItemLightColors = Plugin.Singleton.Config.CustomItemLightColors;

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += DestroyItems;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Server.RoundStarted += SpawningItem;
            //Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= DestroyItems;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Server.RoundStarted -= SpawningItem;
           // Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
        }

        private void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (ActiveLights.ContainsKey(ev.Pickup) && CustomItem.TryGet(ev.Pickup, out CustomItem customItem))
            {
                if (CustomItemLightColors.TryGetValue(customItem!.Id, out var lightColor))
                {
                    if (ev.Throwable == null)
                    {
                        ActiveLights.Remove(ev.Pickup);
                    }
                }
            }
        }
        
        private IEnumerator<float> EnsureDroppedItemLights()
        {
            while (true)
            {
                foreach (var pickup in Pickup.List)
                {
                    if (!ActiveLights.ContainsKey(pickup) && CustomItem.TryGet(pickup, out CustomItem customItem))
                    {
                        if (!CustomItemLightColors.TryGetValue(customItem!.Id, out var lightColor))
                            continue;

                        Timing.RunCoroutine(SpawnLightForPickup(pickup, lightColor));
                    }
                }

                yield return Timing.WaitForOneFrame;
            }
        }
        
        private void SpawningItem()
        {
            Timing.RunCoroutine(EnsureDroppedItemLights());
        }
        
        private void DestroyItems(RoundEndedEventArgs ev)
        {
            foreach (var pickup in Pickup.List)
            {
                if (!ActiveLights.ContainsKey(pickup) && CustomItem.TryGet(pickup, out CustomItem customItem))
                {
                    if (ActiveLights.TryGetValue(pickup, out var light))
                    {
                        light.Destroy();
                        ActiveLights.Remove(pickup);
                    }
                }
            }
        }

        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup == null || !ActiveLights.ContainsKey(ev.Pickup))
                return;

            Log.Info($"Player {ev.Player.Nickname} picked up an item at {ev.Pickup.Position}.");
            
            if (ActiveLights.TryGetValue(ev.Pickup, out var light))
            {
                light.Destroy();
                ActiveLights.Remove(ev.Pickup);
            }
        }

        private IEnumerator<float> SpawnLightForPickup(Pickup pickup, Color lightColor)
        {
            if (pickup == null)
                yield break;

            var light = Light.Create(pickup.Position);
            light.Color = lightColor;
            light.Range = 5;
            light.Intensity = 3;
            light.ShadowType = LightShadows.Hard;
            ActiveLights[pickup] = light;

            Log.Info($"Spawned light for pickup at {pickup.Position} with color {lightColor}.");

            while (pickup.GameObject != null && ActiveLights.ContainsKey(pickup))
            {
                light.Position = new Vector3(pickup.Position.x, pickup.Position.y + 0.2f, pickup.Position.z);
                yield return Timing.WaitForSeconds(0.1f);
            }

            if (ActiveLights.TryGetValue(pickup, out var activeLight))
            {
                activeLight.Destroy();
                ActiveLights.Remove(pickup);
            }
        }
    }
}
