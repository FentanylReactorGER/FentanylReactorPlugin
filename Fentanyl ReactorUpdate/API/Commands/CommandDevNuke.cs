using System;
using CommandSystem;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Classes;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.Commands
{
    public class CommandDevNuke
    {
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class DevNuke : ICommand
        {
            public string Command => "DevNuke";
            public string[] Aliases => Array.Empty<string>();
            public string Description => "Starts an Dev Nuke.";

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                if (!Plugin.Singleton.Config.Devnuke)
                {
                    response = "The Devnuke is disabled.";
                    return false;
                }
                if (!Round.IsStarted)
                {
                    response = "The round has not started yet. Devnuke cannot be triggered.";
                    return false;
                }
                Plugin.Singleton.DevNuke.StartDevNuke();
                response = "NUKING";
                return true;
            }
        }
    }
}