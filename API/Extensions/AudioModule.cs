using System.Linq;
using System.Numerics;
using CommandSystem.Commands.RemoteAdmin.PermissionsManagement.Group;
using Exiled.API.Features;
using Vector3 = UnityEngine.Vector3;

namespace Fentanyl_ReactorUpdate.API.Extensions;

public static class AudioModule
{
    public static void PlayFentanylReactorAudio(this Player player)
    {
        AudioPlayer playerFent = AudioPlayer.Create($"Player {player.Nickname}");
        
        Speaker speaker = playerFent.AddSpeaker("Fentanyl Reactor", 0.9f, false, 10f, 30f);
        
        speaker.transform.parent = player.GameObject.transform;
        
        playerFent.AddClip("Fentanyl Reactor", 0.85f, false, true);
    }
}