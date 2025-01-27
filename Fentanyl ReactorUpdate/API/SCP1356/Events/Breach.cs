using System.Collections;
using System.Collections.Generic;
using MEC;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Features.Objects;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.SCP1356.Events
{
    public class Breach
    {
        private readonly List<Transform> spawnPointTransforms = new();
        private readonly List<string> spawnPointSchematicNames = new List<string>
        {
            "049SpawnPoint1356",
            "096SpawnPoint1356",
            "079SpawnPoint1356",
            "106SpawnPoint1356",
            "ChkpSpawnPoint1356",
            "MicroSpawnPoint1356",
        };

        public void SubEvents()
        {
            MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned += OnSchematicSpawned;
            Exiled.Events.Handlers.Map.Decontaminating += OnDecontaminating;
        }

        public void UnsubEvents()
        {
            MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
            Exiled.Events.Handlers.Map.Decontaminating -= OnDecontaminating;
        }
        
        private void OnSchematicSpawned(MapEditorReborn.Events.EventArgs.SchematicSpawnedEventArgs ev)
        {
            Log.Info($"Schematic spawned: {ev.Schematic.Name}");

            if (spawnPointSchematicNames.Contains(ev.Schematic.Name))
            {
                Plugin.Singleton.RadiationDamage.IsSCP1356Captured = false;
                spawnPointTransforms.Add(ev.Schematic.transform);
                Log.Info($"{ev.Schematic.Name} registered as a spawn point.");
            }

            foreach (Transform DuckSpawnPoint in ev.Schematic.gameObject.GetComponentsInChildren<Transform>())
            {
                if (DuckSpawnPoint.name.Contains("FentanylReactorSpawnPoint1356"))
                {
                    spawnPointTransforms.Add(DuckSpawnPoint);
                    Log.Info($"Child spawn point registered: {DuckSpawnPoint.name}");
                }
            }
        }
        
        private void OnDecontaminating(Exiled.Events.EventArgs.Map.DecontaminatingEventArgs ev)
        {
            Timing.CallDelayed(30f, () =>
            {
                if (!Warhead.IsInProgress && !Warhead.IsDetonated && Round.InProgress)
                {
                    Log.Info("Starting SCP-1356 breach after decontamination.");
                    Plugin.Singleton.SCP1356Breach = true;
                    StartBreach(Plugin.Singleton.RadiationDamage.SCP1356);
                }
                else
                {
                    Log.Warn("Conditions for SCP-1356 breach were not met.");
                }
            });
        }
        
        public void StartBreach(SchematicObject scp1356)
        {
            if (scp1356 == null)
            {
                Log.Error("SCP-1356 schematic is null!");
                return;
            }

            if (spawnPointTransforms.Count == 0)
            {
                Log.Warn("No spawn points have been added. Breach cannot start!");
                return;
            }

            Log.Info("Starting SCP-1356 breach process.");
            Cassie.MessageTranslated(Plugin.Singleton.Translation.SCP1356CassieMessageBreach, Plugin.Singleton.Translation.SCP1356CassieMessageTranslatedBreach
            );
            Timing.RunCoroutine(BreachCoroutine(scp1356));
        }
        
        private IEnumerator<float> BreachCoroutine(SchematicObject scp1356)
        {
            Log.Info($"Starting Breach {Plugin.Singleton.RadiationDamage.IsSCP1356Captured}");
            while (!Plugin.Singleton.RadiationDamage.IsSCP1356Captured)
            {
                var randomIndex = Random.Range(0, spawnPointTransforms.Count);
                var selectedTransform = spawnPointTransforms[randomIndex];
                
                if (selectedTransform.childCount == 0)
                {
                    Log.Warn($"Transform {selectedTransform.name} has no child objects!");
                    continue;
                }

                var firstChild = selectedTransform.GetChild(0);
                firstChild.position.SpecialPos("RadiationWarn.ogg", 15, 25);
                Log.Info($"SCP-1356 will teleport to {selectedTransform.name}'s first child: {firstChild.name}");

                yield return Timing.WaitForSeconds(24f);
                scp1356.Position = firstChild.position;
                Log.Info($"SCP-1356 teleported to {selectedTransform.name}'s first child: {firstChild.name}");

                yield return Timing.WaitForSeconds(60f); 
            }
            Log.Info("Ending Breach");
        }
    }
}
