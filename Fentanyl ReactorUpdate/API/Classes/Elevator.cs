using System.IO;
using System.Runtime.CompilerServices;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Fentanyl_ReactorUpdate.API.Extensions;
using GameCore;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;
using Log = PluginAPI.Core.Log;

namespace Fentanyl_ReactorUpdate.API.Classes;

public class Elevator
{
    public DoorObject Door1356 { get; set; }
    private Animator LeverAnimator { get; set; }
    public void SubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted += OnButtonInteracted;
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned += OnSchematicSpawned;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }
    public void UnsubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted -= OnButtonInteracted;
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }

    private void OnInteractingDoor(Exiled.Events.EventArgs.Player.InteractingDoorEventArgs ev)
    {
        if (ev.Door == Door1356.Door && Door1356.Door != null && Round.IsStarted)
        {
            if (!ev.Door.IsOpen)
            {
                ev.Door.Base.enabled = false;
                ev.Door.Position.SpecialPos("AccesDenied.ogg", 7, 2);
                ev.Player.ShowMeowHint("Schalte die Tür im Kontrollraum frei! \n Nutze dafür den Hebel!");
                ev.Door.Base.enabled = true;
            }
            else if (ev.Door.IsOpen)
            {
                ev.Door.Base.enabled = false;
                ev.Door.Position.SpecialPos("AccesDenied.ogg", 7, 2);
                ev.Player.ShowMeowHint("Die Tür ist bereits offen! \n Nutze den Hebel im Kontrollraum, um sie zu schließen!");
                ev.Door.Base.enabled = true;
            }
        }
    }
    
    private void OnSchematicSpawned(MapEditorReborn.Events.EventArgs.SchematicSpawnedEventArgs ev)
    {
        foreach (Animator animator in ev.Schematic.GetComponentsInChildren<Animator>())
        {
            if (animator.name == "Lever")
            {
                LeverAnimator = animator;
            }
        }
    }
    public void ToggelLever(Player player, string State)
    {
        if (State == "Close")
        {
            if (!Door1356.Door.IsOpen)
            {
                player.ShowMeowHint("Die Tür ist bereits geschlossen!");
                foreach (Pickup pickup in Pickup.List)
                {
                    if (pickup.Base.name == "1356DoorClose")
                    {
                        pickup.Position.SpecialPos("AccesDenied.ogg", 5, 2);
                    }
                }
            }
            else
            {
                LeverAnimator.Play("LeverAnimatorClose");
                foreach (Pickup pickup in Pickup.List)
                {
                    if (pickup.Base.name == "1356DoorClose")
                    {
                        pickup.Position.SpecialPos("LeverUse.ogg", 5, 2);
                    }
                }
                Door1356.Door.IsOpen = false;
            }  
        }
        else if (State == "Open")
        {
            if (Door1356.Door.IsOpen)
            {
                player.ShowMeowHint("Die Tür ist bereits offen!");
                foreach (Pickup pickup in Pickup.List)
                {
                    if (pickup.Base.name == "1356DoorOpen")
                    {
                        pickup.Position.SpecialPos("AccesDenied.ogg", 5, 2);
                    }
                }
            }
            else
            {
                LeverAnimator.Play("LeverAnimatorOpen");
                foreach (Pickup pickup in Pickup.List)
                {
                    if (pickup.Base.name == "1356DoorOpen")
                    {
                        pickup.Position.SpecialPos("LeverUse.ogg", 5, 2);
                    }
                }
                Door1356.Door.IsOpen = true;
            }
        }
    }
    
    private void OnButtonInteracted(MapEditorReborn.Events.EventArgs.ButtonInteractedEventArgs ev)
    {
        if (ev.Button.Base.name == "1356DoorOpen")
        {
            ev.Button.Base.enabled = true;
            ToggelLever(ev.Player, "Open");
        }
        if (ev.Button.Base.name == "1356DoorClose")
        {
            ev.Button.Base.enabled = true;
            ToggelLever(ev.Player, "Close");
        }
    }
}