using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace Fentanyl_ReactorUpdate.API.CustomItems
{
    public class CustomItemLight
    {
        private Dictionary<Pickup, Light> ActiveLights { get; set; } = new();
        private Dictionary<uint, Color> CustomItemLightColors { get; set; } = new()
        {
            { 88, new Color(1f, 0.41f, 0.15f) }, // A warm, fiery orange Fenntttt 1
            { 89, new Color(0.3f, 1f, 0.3f) },  // Soft lime green Fenttttt 2
            { 90, new Color(0.15f, 0.45f, 1f) }, // Soft sky blue FENT!! 3
            { 20, new Color(1f, 1f, 0.15f) },   // Golden yellow
            { 21, new Color(0.85f, 0.14f, 0.14f) }, // Deep red with a hint of brown
            { 22, new Color(0.8f, 0.5f, 0.2f) },  // Burnt amber
            { 23, new Color(0.6f, 0.1f, 0.5f) },  // Bold magenta
            { 24, new Color(0.95f, 0.7f, 0.1f) }, // Bright sunflower yellow
            { 25, new Color(0.34f, 0.17f, 0.72f) }, // Royal purple
            { 26, new Color(0.14f, 0.8f, 0.72f) }, // Teal blue
            { 27, new Color(0.9f, 0.3f, 0.7f) },  // Vivid pink
            { 28, new Color(0.25f, 0.85f, 0.25f) }, // Fresh mint green
            { 29, new Color(1f, 0.53f, 0.26f) },  // Peach orange SCP 1499
            { 30, new Color(0.43f, 0.75f, 1f) },  // Sky blue with a hint of cyan 
            { 31, new Color(1f, 0.6f, 0f) },    // Bright tangerine PB-42
            { 32, new Color(0.88f, 0.56f, 0.9f) }, // Light lavender C4
            { 34, new Color(0.6f, 0.25f, 0.75f) }, // Deep violet Inf. Piöös
            { 1112, new Color(0.2f, 0.8f, 1f) },  // Vibrant cyan Scramble
            { 1113, new Color(0.7f, 0.35f, 0.98f) }, // Electric purple NVG
            { 1488, new Color(0.99f, 0.85f, 0.25f) } // Bright yellow gold D.U.C.K Con. Device
        };

        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Server.RoundStarted += SpawningItem;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Server.RoundStarted -= SpawningItem;
        }

        private void SpawningItem()
        {
            foreach (Pickup pickup in Pickup.List)
            {
                if (CustomItem.TryGet(pickup, out CustomItem customItem))
                {
                    if (!CustomItemLightColors.TryGetValue(customItem!.Id, out var lightColor))
                        return;
                    Timing.RunCoroutine(SpawnLightForPickup(pickup, lightColor));
                }
            }
        }
        
        private void OnDroppedItem(DroppedItemEventArgs ev)
        {
            if (ev.Pickup == null || !CustomItem.TryGet(ev.Pickup, out CustomItem customItem))
                return;

            if (!CustomItemLightColors.TryGetValue(customItem!.Id, out var lightColor))
                return;

            Log.Info($"Player {ev.Player.Nickname} dropped custom item ID {customItem.Id} at {ev.Pickup.Position}.");
            Timing.RunCoroutine(SpawnLightForPickup(ev.Pickup, lightColor));
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

            var light = Exiled.API.Features.Toys.Light.Create(pickup.Position);
            light.Color = lightColor;
            light.Range = 5;
            light.Intensity = 3;
            light.ShadowType = LightShadows.Hard;
            ActiveLights[pickup] = light;

            Log.Info($"Spawned light for pickup at {pickup.Position} with color {lightColor}.");

            while (pickup != null && pickup.GameObject != null && ActiveLights.ContainsKey(pickup))
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
