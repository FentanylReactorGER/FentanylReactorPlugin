using System;
using System.Linq;
using Exiled.API.Features;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fentanyl_ReactorUpdate.API.Classes;

public static class RoomReplacer
{
    public static void DestroyRoom(Room room)
    {
        foreach (Component componentsInChild in room.gameObject.GetComponentsInChildren<Component>())
        {
            try
            {
                if (componentsInChild.name.Contains("SCP-079") ||
                    componentsInChild.name.Contains("CCTV") ||
                    componentsInChild.name.Contains("GeneratorStructure(Clone)") ||
                    //-------------------------------------------------------------------//
                    componentsInChild.GetComponentsInParent<Component>().Any(c => 
                        c.name.Contains("SCP-079") ||
                        c.name.Contains("CCTV") ||
                        c.name.Contains("GeneratorStructure(Clone)")))
                {
                    //Logs.CoreDebugLog(typeof(RoomReplacerModule), $"Prevent from destroying: [{componentsInChild.name}] [{componentsInChild.tag}] [{componentsInChild.GetType().FullName}]");
                    continue;
                }
                //Logs.CoreDebugLog(typeof(RoomReplacerModule), $"Destroying component: [{componentsInChild.name}] [{componentsInChild.tag}] [{componentsInChild.GetType().FullName}]");
                Object.Destroy(componentsInChild);
            }
            catch { }
        }
    }
    public static SchematicObject ReplaceRoom(Room room, string schemeName, Vector3 pos, Quaternion rot, Vector3 scale, SchematicObjectDataList schematicObjectDataList, bool isStatic)
    {
        if (MapUtils.GetSchematicDataByName(schemeName) == null)
        {
            Log.Error($"Tried to replace room - [{room.Type}] but schematic - [{schemeName}] doesnt exist.");
            return null;
        }
        SchematicObject schematic = null;
        try
        {
            schematic = ObjectSpawner.SpawnSchematic(schemeName, pos, rot, scale, schematicObjectDataList, isStatic);
        }
        catch (Exception exception)
        {
            Log.Error($"Tried replace BaseGame room with Scheme - [{schemeName}] in [{room.Type}], but something is broke.\n{exception}");
        }
        DestroyRoom(room);
        Log.Debug($"Done. Room - [{room.Type}] replaced with [{schemeName}]");
        return schematic;
    }
}