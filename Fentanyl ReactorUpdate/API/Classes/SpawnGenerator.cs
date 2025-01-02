using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs;
using Fentanyl_ReactorUpdate;
using Fentanyl_ReactorUpdate.API.Classes;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using Interactables.Interobjects;
using MapGeneration.Distributors;
using MEC;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class FentGenerator
    {
        private Vector3 basePosition { get; set; }
        private Room RoomForGen { get; set; }
        private Pickup ButtonStage1 { get; set; }
        private Pickup ButtonStage2 { get; set; }
        private Pickup ButtonStage3 { get; set; }
        
        private Pickup ButtonRefill { get; set; }
        private Pickup ButtonDeomCoreName { get; set; }
        
        private static readonly Config Config = Plugin.Singleton.Config;
        public void SubEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            // Exiled.Events.Handlers.Map.PickupDestroyed += OnPickupDestroyed;
        }

        public void UnsubEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            // Exiled.Events.Handlers.Map.PickupDestroyed -= OnPickupDestroyed;
        }

        private void OnPickupDestroyed(Exiled.Events.EventArgs.Map.PickupDestroyedEventArgs ev)
        {
            if (ev.Pickup.Base.name == Config.ButtonStage1Name)
            {
                ButtonStage1 = ev.Pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ButtonStage1.Scale = ev.Pickup.Scale;
                ButtonStage1.Base.name = Config.ButtonStage1Name;
                ButtonStage1.PickupTime = 50000f;
                ButtonStage1.Base.enabled = true;
            }
            if (ev.Pickup.Base.name == Config.ButtonStage2Name)
            {
                ButtonStage2 = ev.Pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ButtonStage2.Scale = ev.Pickup.Scale;
                ButtonStage2.Base.name = Config.ButtonStage2Name;
                ButtonStage2.PickupTime = 50000f;
                ButtonStage2.Base.enabled = true;
            }
            if (ev.Pickup.Base.name == Config.ButtonStage3Name)
            {
                ButtonStage3 = ev.Pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ButtonStage3.Scale = ev.Pickup.Scale;
                ButtonStage3.Base.name = Config.ButtonStage3Name;
                ButtonStage3.PickupTime = 50000f;
                ButtonStage3.Base.enabled = true;
            }
            if (ev.Pickup.Base.name == Config.ButtonRefillName)
            {
                ButtonRefill = ev.Pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ButtonRefill.Scale = ev.Pickup.Scale;
                ButtonRefill.Base.name = Config.ButtonRefillName;
                ButtonRefill.PickupTime = 50000f;
                ButtonRefill.Base.enabled = true;
            }
            if (ev.Pickup.Base.name == Config.ButtonDeomCoreName)
            {
                ButtonDeomCoreName = ev.Pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ButtonDeomCoreName.Scale = ev.Pickup.Scale;
                ButtonDeomCoreName.Base.name = Config.ButtonDeomCoreName;
                ButtonDeomCoreName.PickupTime = 50000f;
                ButtonDeomCoreName.Base.enabled = true;
            }
        }
        
        private void OnWaitingForPlayers()
        {
            RoomForGen = Room.Get(Config.RoomType);
            basePosition = RoomForGen.Position;
            Timing.CallDelayed(3f, CheckAndSpawnGenerator);
        }
        
        private void CheckAndSpawnGenerator()
        {
            int generatorCount = Object.FindObjectsOfType<Scp079Generator>().Length;
            Log.Info($"Spawn generator count: {generatorCount}");
            if (generatorCount == Config.GeneratorMax)
            {
                Vector3 adjustedPosition = new Vector3 (-3.368f, 0.471f, -3.026f);
                Vector3 finalGenPos = RoomForGen.WorldPosition(adjustedPosition);
                
                Quaternion rotation = new Quaternion(0, RoomForGen.Position.y, 0, 0);
                var generator = PrefabHelper.Spawn<Scp079Generator>(PrefabType.GeneratorStructure, finalGenPos, rotation);
                generator.gameObject.name = "Fentanyl Generator";
                if (generator != null)
                {
                    Log.Info($"A third generator was spawned at {adjustedPosition}.");
                }
                else
                {
                    Log.Warn("Failed to spawn the Fourth generator.");
                }
            }
            else
            {
                Log.Info($"No generator spawned. Current count: {generatorCount}");
            }
        }
    }
}
