using System.Linq;
using UnityEngine;
using Fentanyl_ReactorUpdate.API;
using MapEditorReborn.API.Features.Objects;

namespace Fentanyl_ReactorUpdate.API.Extensions;

public static class SchematicColor
{
    private static Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        
        float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

        return new Color(r, g, b);
    }

    public static void ChangeLight(this SchematicObject RoomScheme, Color LightColor)
    {
        foreach (LightSourceObject Light in RoomScheme.gameObject
                     .GetComponentsInChildren<LightSourceObject>()
                     .Where(light => 
                         light.Light.Color != HexToColor("#00800AFF") && 
                         light.Light.Color != HexToColor("#FF1600FF")))   
        {
            Light.Light.Color = LightColor;
        }
    }
    
}