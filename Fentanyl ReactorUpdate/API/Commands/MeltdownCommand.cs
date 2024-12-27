using System;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ForceReactorMeltdownCommand : ICommand
{
    public string Command => Plugin.Singleton.Translation.MeltdownCommandName;
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Forces an immediate Fentanyl Reactor meltdown with a random delay before detonation.";
    
    private bool _isUsed;
    public void ResetUsage() => _isUsed = false;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Reactor meltdown cannot be triggered.";
            return false;
        }

        if (_isUsed) // Check if the command was already used
        {
            response = "This command has already been used this round.";
            return false;
        }

        _isUsed = true; // Mark the command as used
        Plugin.Singleton.Reactor.Meltdown(true);
        response = $"Fentanyl Reactor Meltdown in {Plugin.Singleton.Reactor.RandomDelay + 20f} Seconds";
        return true;
    }
}
