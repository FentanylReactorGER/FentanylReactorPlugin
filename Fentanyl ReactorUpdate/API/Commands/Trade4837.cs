using System;
using CommandSystem;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Trade4837 : ICommand
{
    public string Command => "Trade4837";
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Trade4837.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player player = Player.Get(sender);
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be triggered.";
            return false;
        }
        Plugin.Singleton.Main4837.Trade4837(player);
        response = "Trading...";
        return true;
    }
}