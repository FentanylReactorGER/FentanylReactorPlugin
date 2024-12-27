using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Fentanyl_ReactorUpdate.API.Extensions;
using InventorySystem.Items.Usables;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class FentanylReactorFuelCommand : ICommand
{
    public string Command => Plugin.Singleton.Translation.FuelCommandName;
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Fuels the Fentanyl Reactor for a given player.";

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
        if (Plugin.Singleton.Reactor.IsReactorFueled(player))
        {
            player.ShowMeowHint(Plugin.Singleton.Translation.ReactorAlreadyFueledHint);
            response = "Reactor is already fueled!";
            return false;
        }
        if (player.Items.All(item => item.Type != ItemType.Adrenaline))
        {
            player.ShowMeowHint(Plugin.Singleton.Translation.NoAdrenalineHint);
            response = "Player doesn't have Adrenaline!";
            return false;
        }
        if (!Plugin.Singleton.Reactor.Refill(player))
        {
            response = "Could not remove Adrenaline!";
            return false;
        }
        
        player.ShowMeowHint(Plugin.Singleton.Translation.ReactorFueled);
        response = $"Fentanyl Reactor fueled for Player {player.Nickname}.";
        return true;
    }
}