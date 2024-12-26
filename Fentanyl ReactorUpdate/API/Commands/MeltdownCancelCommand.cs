using System;
using CommandSystem;
using Exiled.API.Features;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class StopReactorMeltdownCommand : ICommand
{
    public string Command => Plugin.Singleton.Translation.MeltdownCancelCommandName;
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Cancels an immediate Fentanyl Reactor meltdown with a random delay before detonation.";
        
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be Cancel.";
            return false;
        }
        Plugin.Singleton.Reactor.EndMeltdown();
        response = $"Meltdown canceled"; 
        return true;
    }
}