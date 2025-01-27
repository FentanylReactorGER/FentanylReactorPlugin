using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Fentanyl_ReactorUpdate.API.SCP4837;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using SSMenuSystem.Features;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace Fentanyl_ReactorUpdate.API.CustomItems;

public class CustomItemSchematic
{
    private Dictionary<Pickup, SchematicObject> ActiveBreads { get; set; } = new();
    private Dictionary<Pickup, Light> ActiveLights { get; set; } = new();
    
    public Dictionary<uint, Color> CustomItemLightColors { get; set; } = new()
    {
        { 1489, new Color(1f, 0.41f, 0.15f)}, 
    };
    public Dictionary<uint, string> CustomItemSchematics { get; set; } = new()
    {
        { 1489, "Bread4837"}, 
    };

     public void SubscribeEvents()
    {
        Exiled.Events.Handlers.Server.RoundEnded += DestroyItems;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
        Exiled.Events.Handlers.Server.RoundStarted += SpawningItem;
    }

    public void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Server.RoundEnded -= DestroyItems;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        Exiled.Events.Handlers.Server.RoundStarted -= SpawningItem;
    }

    private void DestroyItems(RoundEndedEventArgs ev)
    {
        ActiveBreads.Clear();
        ActiveLights.Clear();
        
    }

    private IEnumerator<float> EnsureDroppedItemLights()
    {
        while (true)
        {
            foreach (var pickup in Pickup.List)
            {
                if (!ActiveLights.ContainsKey(pickup) && CustomItem.TryGet(pickup, out CustomItem customItem))
                {
                    if (CustomItemLightColors.TryGetValue(customItem.Id, out var lightColor))
                    {
                        if (CustomItemSchematics.TryGetValue(customItem.Id, out var schemeName))
                        {
                            Timing.RunCoroutine(SpawnBreadSchematicAndLight(pickup, lightColor, schemeName));
                        }
                    }
                }
            }

            yield return Timing.WaitForOneFrame;
        }
    }
    
    private void SpawningItem()
    {
        Timing.RunCoroutine(EnsureDroppedItemLights());
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

        if (ActiveBreads.TryGetValue(ev.Pickup, out var Breads))
        {
            Breads.Destroy();
            ActiveBreads.Remove(ev.Pickup);
        }
    }

    private IEnumerator<float> SpawnBreadSchematicAndLight(Pickup pickup, Color lightColor, string schemeName)
    {
        if (pickup == null)
            yield break;
        
        var schematic = ObjectSpawner.SpawnSchematic(schemeName, pickup.Position, Quaternion.Euler(pickup.Rotation.eulerAngles.x, pickup.Rotation.eulerAngles.y, 0), Vector3.one, MapUtils.GetSchematicDataByName(schemeName), true);

        if (schematic == null)
            yield break;

        Log.Info($"Spawned {schemeName} schematic at {pickup.Position}.");
        
        ActiveBreads[pickup] = schematic;
        
        var light = Exiled.API.Features.Toys.Light.Create(pickup.Position);
        light.Color = lightColor;
        light.Position = new Vector3(pickup.Position.x, pickup.Position.y + 0.2f, pickup.Position.z);
        light.Range = 5;
        light.Intensity = 3;
        light.ShadowType = LightShadows.Hard;
        
        ActiveLights[pickup] = light;

        while (pickup != null && pickup.GameObject != null && ActiveBreads.ContainsKey(pickup))
        {
            schematic.transform.position = pickup.Position;
            var rotation = pickup.Rotation.eulerAngles;
            schematic.transform.rotation = Quaternion.Euler(rotation.x, -rotation.y, 0);
            light.Position = pickup.Position;

            yield return Timing.WaitForOneFrame;
        }
        
        if (ActiveLights.TryGetValue(pickup, out var activeLight))
        {
            activeLight.Destroy();
            ActiveLights.Remove(pickup);
        }

        if (ActiveBreads.TryGetValue(pickup, out var activeBread))
        {
            activeBread.Destroy();
            ActiveBreads.Remove(pickup);
        }
    }
}