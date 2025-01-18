using System;
using CommandSystem;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Contain1356 : ICommand
{
    public string Command => "Contain1356";
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Contain 1356.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1 || !int.TryParse(arguments.At(0), out int playerId))
        {
            response = $"Usage: {Command} <PlayerID>";
            return false;
        }
        Player player = Player.Get(playerId);
        if (player == null)
        {
            response = $"No player found with ID {playerId}.";
            return false;
        }
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be triggered.";
            return false;
        }
        if (!Plugin.Singleton.SCP1356Breach)
        {
            response = "The 1356 breach is not currently running.";
            return false;
        }
        Plugin.Singleton.SCP1356Breach = false;
        SchematicObject SCP1356Object = Plugin.Singleton.RadiationDamage.SCP1356;
        Plugin.Singleton.Contain.ContainSCP1356(player, SCP1356Object);
        response = "Containing...";
        return true;
    }
}