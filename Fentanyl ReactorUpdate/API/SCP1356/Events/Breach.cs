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
        // List to store spawn point schematics' transforms
        private readonly List<Transform> spawnPointTransforms = new();

        // List of valid schematic names for spawn points
        private readonly List<string> spawnPointSchematicNames = new List<string>
        {
            "049SpawnPoint1356",
            "096SpawnPoint1356",
            "079SpawnPoint1356",
            "106SpawnPoint1356",
            "ChkpSpawnPoint1356",
            "MicroSpawnPoint1356",
            "FentanylReactorSpawnPoint1356"
        };

        // Subscribe to events
        public void SubEvents()
        {
            MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned += OnSchematicSpawned;
            Exiled.Events.Handlers.Map.Decontaminating += OnDecontaminating;
        }

        // Unsubscribe from events
        public void UnsubEvents()
        {
            MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
            Exiled.Events.Handlers.Map.Decontaminating -= OnDecontaminating;
        }
        
        private void OnSchematicSpawned(MapEditorReborn.Events.EventArgs.SchematicSpawnedEventArgs ev)
        {
            if (spawnPointSchematicNames.Contains(ev.Schematic.Name))
            {
                spawnPointTransforms.Add(ev.Schematic.transform);
                Log.Info(ev.Schematic.transform.name + " Registered");
            }

            foreach (Transform DuckSpawnPoint in ev.Schematic.gameObject.GetComponentsInChildren<Transform>())
            {
                if (DuckSpawnPoint.name.Contains("FentanylReactorSpawnPoint1356"))
                {
                    spawnPointTransforms.Add(DuckSpawnPoint);
                    Log.Info(DuckSpawnPoint.name + " Registered");
                }
            }
        }
        
        private void OnDecontaminating(Exiled.Events.EventArgs.Map.DecontaminatingEventArgs ev)
        {
            Timing.CallDelayed(30f, () => 
            {
                if (!Warhead.IsInProgress || Warhead.IsDetonated || Round.InProgress)
                {
                    Plugin.Singleton.SCP1356Breach = true;
                    StartBreach(Plugin.Singleton.RadiationDamage.SCP1356);
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
            Cassie.MessageTranslated("pitch_0.7 BELL_START .g4 .g6 pitch_1 SCP 1 3 5 6 containment pitch_0.9 failure pitch_1 detected . Containment status .g3 .g2 pitch_0.7 error .g2 . pitch_1 . continue with pitch_0.9 caution pitch_1 pitch_0.7 .g6 .g4 BELL_END pitch_1", "<color=yellow>SCP-1356</color> Eindämmungsbruch festgestellt. Eindämmungsstatus: <color=red>Fehler.</color> Fahren Sie mit <color=red>Vorsicht</color> fort!");
            Timing.RunCoroutine(BreachCoroutine(scp1356));
        }
        
        private IEnumerator<float> BreachCoroutine(SchematicObject scp1356)
        {
            while (true)
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
                Log.Info($"SCP-1356 will teleported to {selectedTransform.name}'s first child: {firstChild.name}");
                yield return Timing.WaitForSeconds(24f);
                scp1356.Position = firstChild.position;
                Log.Info($"SCP-1356 teleported to {selectedTransform.name}'s first child: {firstChild.name}");
                
                yield return Timing.WaitForSeconds(60f); 
            }
        }
    }
}
