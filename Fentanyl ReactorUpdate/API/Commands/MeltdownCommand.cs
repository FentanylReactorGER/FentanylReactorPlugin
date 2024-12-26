using System;
using CommandSystem;
using Exiled.API.Features;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ForceReactorMeltdownCommand : ICommand
{
    public string Command => Plugin.Singleton.Translation.MeltdownCommandName;
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Forces an immediate Fentanyl Reactor meltdown with a random delay before detonation.";
        
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be triggered.";
            return false;
        }
        Plugin.Singleton.Reactor.Meltdown(true);
        response = $"Fentanyl Reactor Meltdown in {Plugin.Singleton.Reactor.RandomDelay + 20f} Seconds"; // why there is +20f ?
        return true;
    }
}