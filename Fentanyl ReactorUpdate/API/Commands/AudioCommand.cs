using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
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
        
        if (arguments.Count < 3)
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
        string Volume = arguments.At(3);
        AudioMeltdown(BotName, ClipPath, ClipName,Volume);
        response = "Playing Audio...";
        return true;
    }
    public void AudioMeltdown(string BotName, string ClipPath, string ClipName, string Volume)
    {
        float Volumeee = float.Parse(Volume);
        Player randomPlayer = Player.List.ElementAt(UnityEngine.Random.Range(0, Player.List.Count()));
        Vector3 meltdownPos = randomPlayer.Position;
        AudioClipStorage.LoadClip(ClipPath, ClipName);

        CommandPlayer = AudioPlayer.CreateOrGet($"GlobalAudioPlayer", onIntialCreation: (player) =>
        {
            player.AddSpeaker(BotName, meltdownPos, 1f, false, 5f, 5000f);
        });

        foreach (var speaker in CommandPlayer.SpeakersByName)
            speaker.Value.Position = meltdownPos;
        CommandPlayer.AddClip(ClipName, Volumeee, false, true);
        if (CommandPlayer.DestroyWhenAllClipsPlayed)
        {
            CommandPlayer.Destroy();
        }
    }
}