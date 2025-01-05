using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Extensions;
using MEC;
using UnityEngine;
using Object = UnityEngine.Object;
using RandomDelayGiver = Fentanyl_ReactorUpdate.API;

namespace Fentanyl_ReactorUpdate.API.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class AudioCommand : ICommand
{
    public AudioPlayer CommandPlayer { get; set; } 
    public string Command => "AudioPlayerFent";
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Plays an Audio.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player player = Player.Get(sender);
        if (!Round.IsStarted)
        {
            response = "The round has not started yet. Audio cannot be triggered.";
            return false;
        }
        
        if (arguments.Count < 2)
        {
            response = "Usage: AudioPlayerFent <Bot Name> <Clip Name / PATH> <Gib deinem Clip einem Namen> <Volume>";
            return false;
        }

        if (!CommandPlayer.DestroyWhenAllClipsPlayed)
        {
            response = "The command Audio has already been triggered, wait for it to finish or create a new bot with a new Name.";
        }
        string BotName = arguments.At(0);
        string ClipName = arguments.At(1);
        string ClipPath = arguments.At(2);
        player.GlobalPlayer(BotName, ClipName, ClipPath, 5000f);
        response = "Playing Audio...";
        return true;
    }
}