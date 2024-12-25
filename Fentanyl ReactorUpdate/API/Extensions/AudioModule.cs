using System.Linq;
using System.Numerics;
using CommandSystem.Commands.RemoteAdmin.PermissionsManagement.Group;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using Vector3 = UnityEngine.Vector3;

namespace Fentanyl_ReactorUpdate.API.Extensions;

public static class AudioModule
{
    public static void PlayFentanylReactorAudio(this Player player)
    {
        AudioPlayer playerFent = AudioPlayer.Create($"Player {player.Nickname}");
        
        Speaker speaker = playerFent.AddSpeaker("Fentanyl Reactor", Plugin.Singleton.Config.FentanylReactorAudioVolume, false, Plugin.Singleton.Config.FentanylReactorAudioMin, Plugin.Singleton.Config.FentanylReactorAudioMax);
        
        speaker.Position = player.Position;
        
        playerFent.AddClip("Fentanyl Reactor", Plugin.Singleton.Config.FentanylReactorAudioVolume, false, true);
    }
}