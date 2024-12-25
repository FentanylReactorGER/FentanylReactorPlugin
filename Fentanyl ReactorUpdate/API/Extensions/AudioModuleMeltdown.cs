using System.Linq;
using System.Numerics;
using CommandSystem.Commands.RemoteAdmin.PermissionsManagement.Group;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using Vector3 = UnityEngine.Vector3;

namespace Fentanyl_ReactorUpdate.API.Extensions;

public static class AudioModuleMeltdown
{
    public static void PlayFentanylReactorMeltdownAudio()
    {
        AudioPlayer globalPlayer = AudioPlayer.Create("GlobalAudioPlayer");
        
        Speaker speaker = globalPlayer.AddSpeaker("GlobalSpeaker");
        
        globalPlayer.AddClip("Fentanyl Reactor Meltdown", Plugin.Singleton.Config.FentanylReactorAudioVolume, false, true);
    }
}