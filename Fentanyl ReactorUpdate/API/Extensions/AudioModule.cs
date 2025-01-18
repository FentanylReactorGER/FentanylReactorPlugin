using System;
using System.Collections.Generic;
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
    private static bool _PlayingAudio { get; set; }

    public static void SpecialPos(this Vector3 Positon, string ClipPath, float MaxDistance, float ClipDuration)
    {
        string globalPlayerName = GenerateRandomString(6);
        string speakerName = GenerateRandomString(6);
        string ClipName = GenerateRandomString(6);
        
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "audio", ClipPath), ClipName);

        AudioPlayer MassivePlayer = AudioPlayer.Create(globalPlayerName);
        Speaker speaker = MassivePlayer.AddSpeaker(speakerName, 1f, true, 1, MaxDistance);

        speaker.Position = Positon;

        MassivePlayer.AddClip(ClipName, 1f, false, true);

        Timing.CallDelayed(ClipDuration, () => 
        {
            if (!Round.IsEnded)
            {
                AudioClipStorage.DestroyClip(ClipName);
                speaker.Destroy();
                MassivePlayer.RemoveSpeaker(speakerName);
                MassivePlayer.Destroy();
            }
        });

        if (Round.IsEnded)
        {
            AudioClipStorage.DestroyClip(ClipName);
            speaker.Destroy();
            MassivePlayer.RemoveSpeaker(speakerName);
            MassivePlayer.Destroy();
        }
    }
    public static void SpecialPosExtra(this Vector3 positon, string ClipPath, float MaxDistance, float Volume, float duration)
    {
        string globalPlayerName = GenerateRandomString(6);
        string speakerName = GenerateRandomString(6);
        string ClipName = GenerateRandomString(6);
        
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "audio", ClipPath), ClipName);

        AudioPlayer MassivePlayer = AudioPlayer.Create(globalPlayerName);
        Speaker speaker = MassivePlayer.AddSpeaker(speakerName, 1f, true, 1, MaxDistance);
        
        speaker.Position = positon;

        MassivePlayer.AddClip(ClipName, Volume, false, true);

        Timing.CallDelayed(duration, () => 
        {
            if (!Round.IsEnded)
            {
                AudioClipStorage.DestroyClip(ClipName);
                speaker.Destroy();
                MassivePlayer.RemoveSpeaker(speakerName);
                MassivePlayer.Destroy();
            }
        });
        
        if (Round.IsEnded)
        {
            AudioClipStorage.DestroyClip(ClipName);
            speaker.Destroy();
            MassivePlayer.RemoveSpeaker(speakerName);
            MassivePlayer.Destroy();
        }
    }
    public static void FentanylAudio(this Player player, string ClipPath, float MaxDistance, float Volume, float duration)
    {
        string globalPlayerName = GenerateRandomString(6);
        string speakerName = GenerateRandomString(6);
        string ClipName = GenerateRandomString(6);
        
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "audio", ClipPath), ClipName);

        AudioPlayer MassivePlayer = AudioPlayer.Create(globalPlayerName);
        Speaker speaker = MassivePlayer.AddSpeaker(speakerName, 1f, true, 1, MaxDistance);
        
        speaker.Position = player.Position;
        _PlayingAudio = true;
        Timing.RunCoroutine(PosForSpeaker(speaker, player, duration));

        MassivePlayer.AddClip(ClipName, Volume, false, true);

        Timing.CallDelayed(duration, () => 
        {
            if (!Round.IsEnded)
            {
                _PlayingAudio = false;
                AudioClipStorage.DestroyClip(ClipName);
                speaker.Destroy();
                MassivePlayer.RemoveSpeaker(speakerName);
                MassivePlayer.Destroy();
            }
        });
        
        if (Round.IsEnded)
        {
            _PlayingAudio = false;
            AudioClipStorage.DestroyClip(ClipName);
            speaker.Destroy();
            MassivePlayer.RemoveSpeaker(speakerName);
            MassivePlayer.Destroy();
        }
    }

    private static IEnumerator<float> PosForSpeaker(Speaker speaker, Player player, float Duration)
    {
        Log.Info("kys1");
        while (_PlayingAudio)
        {
            speaker.transform.position = player.Transform.position;
            yield return Timing.WaitForOneFrame;
        }
        Log.Info("kys2");
        yield break;
    }
    
    public static void MassivePlayer(this Player player, string ClipPath, float MaxDistance, float ClipDuration)
    {
        string globalPlayerName = GenerateRandomString(6);
        string speakerName = GenerateRandomString(6);
        string ClipName = GenerateRandomString(6);
        
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "audio", ClipPath), ClipName);

        AudioPlayer MassivePlayer = AudioPlayer.Create(globalPlayerName);
        Speaker speaker = MassivePlayer.AddSpeaker(speakerName, 1f, false, 1, MaxDistance);

        speaker.Position = player.Position;

        MassivePlayer.AddClip(ClipName, 1f, false, true);

        Timing.CallDelayed(ClipDuration, () => 
        {
            if (!Round.IsEnded)
            {
                AudioClipStorage.DestroyClip(ClipName);
                speaker.Destroy();
                MassivePlayer.RemoveSpeaker(speakerName);
                MassivePlayer.Destroy();
            }
        });

        if (Round.IsEnded)
        {
            AudioClipStorage.DestroyClip(ClipName);
            speaker.Destroy();
            MassivePlayer.RemoveSpeaker(speakerName);
            MassivePlayer.Destroy();
        }
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();
        char[] stringChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}

public class AudioPlayerRandom
{
    public void GlobalPlayer(string ClipPath, float MaxDistance, float ClipDuration)
    {
        string globalPlayerName = GenerateRandomString(6);
        string speakerName = GenerateRandomString(6);
        string ClipName = GenerateRandomString(6);
        
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "audio", ClipPath), ClipName);

        AudioPlayer MassivePlayer = AudioPlayer.Create(globalPlayerName);
        Speaker speaker = MassivePlayer.AddSpeaker(speakerName, 1f, false, 1, MaxDistance);

        Player randomPlayer = Player.List.ElementAt(UnityEngine.Random.Range(0, Player.List.Count()));
        
        speaker.Position = randomPlayer.Position;

        MassivePlayer.AddClip(ClipName, 1f, false, true);

        Timing.CallDelayed(ClipDuration, () => 
        {
            if (!Round.IsEnded)
            {
                AudioClipStorage.DestroyClip(ClipName);
                speaker.Destroy();
                MassivePlayer.RemoveSpeaker(speakerName);
                MassivePlayer.Destroy();
            }
        });

        if (Round.IsEnded)
        {
            AudioClipStorage.DestroyClip(ClipName);
            speaker.Destroy();
            MassivePlayer.RemoveSpeaker(speakerName);
            MassivePlayer.Destroy();
        }
    }
    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();
        char[] stringChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}