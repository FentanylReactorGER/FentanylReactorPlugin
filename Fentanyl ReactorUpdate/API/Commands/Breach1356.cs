using System;
using CommandSystem;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Breach1356 : ICommand
{
    public string Command => "Breach1356";
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Breach 1356.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player player = Player.Get(sender);
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be triggered.";
            return false;
        }

        if (Plugin.Singleton.SCP1356Breach)
        {
            response = "The 1356 breach is currently running.";
            return false;
        }
        Plugin.Singleton.SCP1356Breach = true;
        SchematicObject SCP1356Object = Plugin.Singleton.RadiationDamage.SCP1356;
        Plugin.Singleton.Breach.StartBreach(SCP1356Object);
        response = "Breaching...";
        return true;
    }
}