using System.IO;
using System.Linq;
using System.Numerics;
using CommandSystem.Commands.RemoteAdmin.PermissionsManagement.Group;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using Vector3 = UnityEngine.Vector3;

namespace Fentanyl_ReactorUpdate.API.Extensions;

public static class AudioModule
{
    
    // Jjaja hier sind 2 Audio module
    public static void PlayFentanylReactorAudio(this Player player)
    {
        AudioPlayer playerFent = AudioPlayer.Create($"Player {player.Nickname}");
        
        Speaker speaker = playerFent.AddSpeaker("Fentanyl Reactor", Plugin.Singleton.Config.FentanylReactorAudioVolume, false, Plugin.Singleton.Config.FentanylReactorAudioMin, Plugin.Singleton.Config.FentanylReactorAudioMax);
        
        speaker.Position = player.Position;
        
        playerFent.AddClip("Fentanyl Reactor", Plugin.Singleton.Config.FentanylReactorAudioVolume, false, true);
        Timing.CallDelayed(15f,
            () =>
            
            {
                playerFent.RemoveSpeaker("Fentanyl Reactor");
                playerFent.Destroy();
            });
    }
    public static void GlobalPlayer(this Player player, string SpeakerName, string ClipName, string ClipPath, float MaxDistance)
    {
        AudioPlayer GlobalPlayer = AudioPlayer.Create($"Player {player.Nickname}");
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "DEVNUKE.ogg"), ClipName);
        Speaker speaker = GlobalPlayer.AddSpeaker(SpeakerName, Plugin.Singleton.Config.FentanylReactorAudioVolume, false, 5, MaxDistance);
        
        speaker.Position = player.Position;
        
        GlobalPlayer.AddClip(ClipName, 1f, false, true);
        if (Round.IsEnded)
        {
            GlobalPlayer.RemoveSpeaker(SpeakerName);
            GlobalPlayer.Destroy();
        }
    }
}