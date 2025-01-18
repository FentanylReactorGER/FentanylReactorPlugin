using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using Interactables.Interobjects.DoorUtils;
using MapEditorReborn.API.Enums;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using MapGeneration.Distributors;
using MEC;
using UnityEngine;
using KeycardPermissions = Exiled.API.Enums.KeycardPermissions;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class FentGenerator
    {
        private DoorObject DoorType1 { get; set; }
        private DoorObject DoorType2 { get; set; }
        private DoorObject DoorType3 { get; set; }
        private DoorObject DoorType4 { get; set; }
        private DoorObject Door1356Chamber { get; set; } = Plugin.Singleton.Elevator.Door1356;
        
        private static readonly Config Config = Plugin.Singleton.Config;

        public void SubEvents()
        {
            MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned += OnSchematicSpawned;
            Exiled.Events.Handlers.Warhead.Starting += OnStarting;
            Exiled.Events.Handlers.Warhead.Stopping += OnStopping;
        }

        public void UnsubEvents()
        {
            MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
            Exiled.Events.Handlers.Warhead.Starting -= OnStarting;
            Exiled.Events.Handlers.Warhead.Stopping -= OnStopping;

        }
        private List<DoorObject> doorList;
        private void OnStarting(Exiled.Events.EventArgs.Warhead.StartingEventArgs ev)
        {
            doorList = new List<DoorObject>
            {
                DoorType1,
                DoorType2,
                DoorType3,
                DoorType4,
                Door1356Chamber
            };
            Timing.CallDelayed(15, () => 
            {
                if (!Round.IsEnded)
                {
                    foreach (DoorObject door in doorList)
                    {
                        if (!door.Door.IsOpen)
                        {
                            door.Door.IsOpen = true;
                        }
                        door.Door.Lock(DoorLockType.Warhead);
                    }
                }
            });
        }

        private void OnStopping(Exiled.Events.EventArgs.Warhead.StoppingEventArgs ev)
        {
            doorList = new List<DoorObject>
            {
                DoorType1,
                DoorType2,
                DoorType3,
                DoorType4,
                Door1356Chamber
            };
            foreach (DoorObject door in doorList)
            {
                door.Door.Unlock();
            }
        }

        public void CallCustomDoorLockdown()
        {
            doorList = new List<DoorObject>
            {
                DoorType1,
                DoorType2,
                DoorType3,
                DoorType4,

            };
            Timing.CallDelayed(5, () => 
            {
                if (!Round.IsEnded)
                {
                    foreach (DoorObject door in doorList)
                    {
                        if (!door.Door.IsOpen)
                        {
                            door.Door.IsOpen = true;
                        }
                        door.Door.Lock(DoorLockType.Warhead);
                    }
                }
            });
        }
        
        private void CheckAndSpawnGenerator(Vector3 Position, Quaternion Rotation)
        {
            PrefabHelper.Spawn<Scp079Generator>(PrefabType.GeneratorStructure, Position, Rotation);
        }

        public void Spawn1356Door(Vector3 position, Quaternion rotation, DoorType doorType)
        {
            Vector3 scale = new Vector3(1, 1, 1);
            DoorSerializable door = new DoorSerializable
            {
                KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions.None,
                IsLocked = true,
                IgnoredDamageSources = DoorDamageType.Grenade | DoorDamageType.ParticleDisruptor,
                DoorHealth = 15000,
                DoorType = doorType  
            };
            
            Plugin.Singleton.Elevator.Door1356 = ObjectSpawner.SpawnDoor(door, position, rotation, scale);
        }

        private void SpawnEZDoor(Vector3 position, Quaternion rotation, DoorType doorType, Transform transform, string Name)
        {
            Log.Info(transform.rotation.eulerAngles);
            Vector3 scale = new Vector3(1, 1, 1);
            DoorSerializable door = new DoorSerializable
            {
                DoorType = doorType  
            };
            if (Name == "Door1")
            {
                DoorType1 = ObjectSpawner.SpawnDoor(door, position, rotation, scale);
            }
            if (Name == "Door2")
            {
                DoorType2 = ObjectSpawner.SpawnDoor(door, position, rotation, scale);
            }
            if (Name == "Door3")
            {
                DoorType3 = ObjectSpawner.SpawnDoor(door, position, rotation, scale);
            }
            if (Name == "Door4")
            {
                DoorType4 = ObjectSpawner.SpawnDoor(door, position, rotation, scale);
            }
        }

        private void OnSchematicSpawned(MapEditorReborn.Events.EventArgs.SchematicSpawnedEventArgs ev)
        {
            Transform[] allChildren = ev.Schematic.gameObject.GetComponentsInChildren<Transform>();

            string targetName = "GeneratorFentanyl";
            foreach (Transform childTransform in allChildren)
            {
                if (childTransform.gameObject.name == targetName)
                {
                    if (Generator.List.Count != 3)
                    {
                        CheckAndSpawnGenerator(childTransform.position, childTransform.rotation);
                    }
                }
            }

            string DoorNameType1 = "DoorEntranceType1";
            foreach (Transform childTransform in allChildren)
            {
                if (childTransform.gameObject.name == DoorNameType1)
                {
                    SpawnEZDoor(childTransform.position, childTransform.rotation, DoorType.HeavyContainmentDoor, childTransform, "Door1");
                    DoorType1.Door.KeycardPermissions = KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride;
                }
            }
            string DoorNameType2 = "DoorEntranceType2";
            foreach (Transform childTransform in allChildren)
            {
                if (childTransform.gameObject.name == DoorNameType2)
                {
                    SpawnEZDoor(childTransform.position, childTransform.rotation, DoorType.HeavyContainmentDoor, childTransform, "Door2");
                    DoorType2.Door.KeycardPermissions = KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride;
                }
            }
            string DoorNameType3 = "DoorEntranceType3";
            foreach (Transform childTransform in allChildren)
            {
                if (childTransform.gameObject.name == DoorNameType3)
                {
                    SpawnEZDoor(childTransform.position, childTransform.rotation, DoorType.HeavyContainmentDoor, childTransform, "Door3");
                    DoorType3.Door.KeycardPermissions = KeycardPermissions.ContainmentLevelTwo | KeycardPermissions.ScpOverride;
                }
            }
            string DoorNameType4 = "DoorEntranceType4";
            foreach (Transform childTransform in allChildren)
            {
                if (childTransform.name == DoorNameType4)
                {
                    SpawnEZDoor(childTransform.position, childTransform.rotation, DoorType.HeavyContainmentDoor, childTransform, "Door4");
                    DoorType4.Door.KeycardPermissions = KeycardPermissions.ContainmentLevelTwo & KeycardPermissions.ArmoryLevelOne;
                    Log.Info($"{childTransform.rotation.eulerAngles} | {childTransform.position}");
                }
            }
            string DoorSCP1356 = "Door1356";
            foreach (Transform childTransform in allChildren)
            {
                if (childTransform.gameObject.name == DoorSCP1356)
                {
                    Spawn1356Door(childTransform.position, childTransform.rotation, DoorType.HeavyContainmentDoor);
                }
            }
        }
    }
}