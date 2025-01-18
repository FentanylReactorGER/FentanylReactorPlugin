using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Extensions;
using MEC;
using UnityEngine;
using Object = UnityEngine.Object;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ButtonCommand : ICommand
{
    public string Command => "ButtonUse";
    public string[] Aliases => Array.Empty<string>();
    public string Description => "uses Button (OWNER ONLY)";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 2 || !(arguments[1] == "Close" || arguments[1] == "Open"))
        {
            response = $"Usage: {Command} <PlayerID> <Close|Open>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId))
        {
            response = "Invalid PlayerID!";
            return false;
        }
        Player player = Player.Get(playerId);
        if (player == null)
        {
            response = $"No player found with ID {playerId}.";
            return false;
        }
        string ButtonState = arguments.At(1);
        Plugin.Singleton.Elevator.ToggelLever(player, ButtonState);
        response = "Playing Audio...";
        return true;
    }
}