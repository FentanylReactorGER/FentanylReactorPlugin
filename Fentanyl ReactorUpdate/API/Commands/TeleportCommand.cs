using System;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class TeleportFentanyl : ICommand
{
    public string Command => Plugin.Singleton.Translation.TeleportFentanyl;
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Teleports you to the Fentanyl Reactor.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player player = Player.Get(sender);
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be triggered.";
            return false;
        }

        var RoomPos = new Vector3(Plugin.Singleton.Reactor.RoomScheme.Position.x, Plugin.Singleton.Reactor.RoomScheme.Position.y + 3, Plugin.Singleton.Reactor.RoomScheme.Position.z);
        player.Position = RoomPos;
        response = "Teleporting...";
        return true;
    }
}