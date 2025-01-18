using System;
using CommandSystem;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Reset : ICommand
{
    public string Command => "Resetcooldown4837";
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

        Plugin.Singleton.Main4837._Cooldown = false;
        response = "Breaching...";
        return true;
    }
}