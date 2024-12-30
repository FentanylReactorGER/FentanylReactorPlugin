using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using Exiled.API.Enums;
using Fentanyl_ReactorUpdate.API.Classes;
using MapEditorReborn.API.Features.Objects;
using UnityEngine;

namespace TeleportPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TeleportCommand : ICommand
    {
        public string Command => "roomreplacer";

        public string[] Aliases => new[] { "rroom" };

        public string Description => "Replaces a specific room.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("roomreplacer.replace"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
                
            if (arguments.Count < 2)
            {
                response = "Usage: replace_room <room_name> <schematic_name>";
                return false;
            }

            string roomName = arguments.At(0);
            string schematicName = arguments.At(1);
            
            RoomType roomType;
            if (!Enum.TryParse(roomName, true, out roomType))
            {
                response = $"Invalid room name '{roomName}'. Please provide a valid room type.";
                return false;
            }
            
            Room room = Room.Get(roomType);
            if (room == null)
            {
                response = $"Room '{roomName}' not found.";
                return false;
            }

            try
            {
                Vector3 position = room.Position;
                Quaternion rotation = room.Transform.rotation;
                Vector3 scale = Vector3.one;
                RoomReplacer.ReplaceRoom(room, schematicName, position, rotation, scale, null, true);

                response = $"Room '{roomName}' successfully replaced with schematic '{schematicName}'.";
                return true;
            }
            catch (Exception ex)
            {
                response = $"An error occurred while replacing the room: {ex.Message}";
                return false;
            }
        }
    }
}
