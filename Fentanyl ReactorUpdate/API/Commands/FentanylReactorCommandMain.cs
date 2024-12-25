using System;
using CommandSystem;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class FentanylReactorCommand : ICommand
{
    public string Command => Plugin.Singleton.Translation.CommandName;
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Starts the Fentanyl Reactor with a given level.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 2 || !int.TryParse(arguments.At(1), out int level) || level < 1 || level > 3)
        {
            response = $"Usage: {Command} <PlayerID> <Level (1-3)>";
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
        if (!Plugin.Singleton.Reactor.IsReactorFueled(player))
        {
            player.ShowMeowHint(Plugin.Singleton.Translation.ReactorNotFueledHint);
            response = "Reactor is not fueled!";
            return false;
        }
        if (!Plugin.Singleton.Reactor.CanUseReactor(player, out double remainingTime))
        {
            response = $"You need to wait {Math.Ceiling(remainingTime)} seconds before using the Fentanyl Reactor again.";
            player.ShowMeowHint($"{Plugin.Singleton.Translation.ReactorCooldown} {Math.Round(remainingTime)} Sekunden");
            return false;
        }
        
        player.ShowMeowHint(Plugin.Singleton.Translation.ReactorStartingHint);
        response = Plugin.Singleton.Reactor.Start(player, level);
        return true;
    }
}