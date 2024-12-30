using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.Doors;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Loader;
using Fentanyl_ReactorUpdate.API.Commands;
using Fentanyl_ReactorUpdate.API.Extensions;
using LightContainmentZoneDecontamination;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.Events.EventArgs;
using MEC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fentanyl_ReactorUpdate.API;

public class Reactor
{
    public bool IsReplace => Plugin.Singleton.Config.ReplaceRoom;
    public RoomType RoomType => Plugin.Singleton.Config.RoomType;
    public int CommandCooldown => Plugin.Singleton.Config.CommandCooldown;
    public string SchematicName => Plugin.Singleton.Config.SchematicName;
    public float MeltdownStart => Plugin.Singleton.Config.MeltdownZeitStart;
    public float MeltdownEnd => Plugin.Singleton.Config.MeltdownZeitEnd;
    public string CassieMessage => Plugin.Singleton.Translation.FentanylReactorMeltdownCassie;
    public string CassieTranslation => Plugin.Singleton.Translation.FentanylReactorMeltdownCassieTrans;
    
    
    public float RandomDelay { get; private set; }
    public Room Room { get; private set; }
    
    public SchematicObject RoomScheme { get; private set; }

    public Reactor()
    { 
        SubEvents();
    }

    public void AudioMeltdown()
    {
        Player randomPlayer = Player.List.ElementAt(UnityEngine.Random.Range(0, Player.List.Count()));
        Vector3 meltdownPos = randomPlayer.Position;

        AudioPlayer globalPlayer = AudioPlayer.CreateOrGet($"GlobalAudioPlayer", onIntialCreation: (player) =>
        {
            player.AddSpeaker("Reactor Meltdown", meltdownPos, 1f, false, 5f, 5000f);
        });

        foreach (var speaker in globalPlayer.SpeakersByName)
            speaker.Value.Position = meltdownPos;
        globalPlayer.AddClip("Fentanyl Reactor Meltdown", 1f, false, true);
        Timing.CallDelayed(RandomDelay + 20f,
            () =>
            {
                globalPlayer.RemoveAllClips();
                globalPlayer.Destroy();
                globalPlayer.RemoveSpeaker("Fentanyl Reactor Meltdown");
            });
    }
    
    public void Destroy()
    {
        UnSubEvents();
        Timing.KillCoroutines(_metldownProcess);
        Warhead.Stop();
        Warhead.IsLocked = false;
    }
    
    public Dictionary<Player, DateTime> Cooldowns = new Dictionary<Player, DateTime>();
    public HashSet<Player> FueledReactors = new HashSet<Player>();

    private CoroutineHandle _metldownProcess;

    #region Events

    public void SubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted += OnButtonInteracted;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingForPlayers;
    }

    public void UnSubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted -= OnButtonInteracted;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingForPlayers;
    }

    private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
    {
        EndMeltdown();
        _meltdownTriggered = true;
        Plugin.Singleton.MeltdownCommandInstance.ResetUsage();
        Warhead.Stop();
        Warhead.IsLocked = false;
        if (AudioPlayer.AudioPlayerByName.TryGetValue("GlobalAudioPlayer", out AudioPlayer ap))
        {
            ap.RemoveAllClips();
        }
    }

    
    private bool _meltdownTriggered = false;
    
    public bool IsMeltdownTriggered => _meltdownTriggered;
    
    public void SetMeltdownTriggered(bool triggered)
    {
        _meltdownTriggered = true;
    }

    private void WaitingForPlayers()
    {
        _meltdownTriggered = true;
        if (Round.LobbyWaitingTime == 5)
        {
            _meltdownTriggered = false;
        }
    }
    
    private void OnRoundStarted()
    {
        _meltdownTriggered = false;
        RandomDelay = UnityEngine.Random.Range(MeltdownStart, MeltdownEnd);
        Room room = Room.Get(RoomType);
        if (room == null)
        {
            Log.Info("Room not found! Plugin is disabled.");
            Destroy();
            return;
        }
        
        Room = room;

        if (!IsReplace)
        {
            Log.Info("Room Replacer ist Deaktiviert");
            return;
        }
        Warhead.Stop();
        Warhead.IsLocked = false;
        RoomScheme = API.Classes.RoomReplacer.ReplaceRoom(room, SchematicName, room.Position, room.Rotation,
            Vector3.one, MapUtils.GetSchematicDataByName(SchematicName), false);
    }
    
    #endregion
    
    public string Start(Player player, int level)
    {
        if (!IsReactorFueled(player)) return "Reactor is not fueled!";
        if (!CanUseReactor(player, out double remainingTime))
            return $"You need to wait {Math.Ceiling(remainingTime)} seconds before using the Fentanyl Reactor again.";

        ApplyCooldown(player);
        ConsumeReactorFuel(player);

        float chance;
        CustomItem customItem;
        string successHint;
        string failureHint = Plugin.Singleton.Translation.ReactorFailureHint;

        switch (level)
        {
            case 1:
                chance = Plugin.Singleton.Config.Level1Chance;
                customItem = CustomItem.Get(Plugin.Singleton.Config.T1ID);
                successHint = Plugin.Singleton.Translation.ReactorSuccessHintStageOne;
                break;
            case 2:
                chance = Plugin.Singleton.Config.Level2Chance;
                customItem = CustomItem.Get(Plugin.Singleton.Config.T2ID);
                successHint = Plugin.Singleton.Translation.ReactorSuccessHintStageTwo;
                break;
            case 3:
                chance = Plugin.Singleton.Config.Level3Chance;
                customItem = CustomItem.Get(Plugin.Singleton.Config.T3ID);
                successHint = Plugin.Singleton.Translation.ReactorSuccessHintStageThree;
                break;
            default:
                return "Invalid reactor level.";
        }

        if (customItem == null) return $"Cant get custom item for level {level}";

        if (player.Nickname.Equals("Fentanyl Reactor", StringComparison.OrdinalIgnoreCase))
        {
            player.PlayFentanylReactorAudio();
            Timing.CallDelayed(Plugin.Singleton.Config.ReactorWaitTime,
                () =>
                {
                    player.ShowMeowHint("Willkommen, Fentanyl Reaktor! \n Ihr Fentanyl ist auf dem Weg....");
                });
            Timing.CallDelayed(Plugin.Singleton.Config.ReactorWaitTime + Plugin.Singleton.Config.GlobalHintDuration,
                () =>
                {
                    customItem.Give(player);
                });
            return $"Fentanyl Reactor fully fueled for Player {player.Nickname}.";
        }

        if (UnityEngine.Random.Range(1, 101) <= chance)
        {
            player.PlayFentanylReactorAudio();
            Timing.CallDelayed(Plugin.Singleton.Config.ReactorWaitTime,
                () =>
                {
                    player.ShowMeowHint(successHint);
                });
            Timing.CallDelayed(Plugin.Singleton.Config.ReactorWaitTime + Plugin.Singleton.Config.GlobalHintDuration,
                () =>
                {
                    customItem.Give(player);
                });
            return $"Fentanyl Reactor succeeded for Player {player.Nickname} at Level {level}.";
        }
        Log.Info($"X: {RoomScheme.Position.x} Y: {RoomScheme.Position.y} Z: {RoomScheme.Position.z}");
        player.PlayFentanylReactorAudio();
        Timing.CallDelayed(Plugin.Singleton.Config.ReactorWaitTime,
            () =>
            {
                player.ShowMeowHint(failureHint);
            });
        return $"Fentanyl Reactor failed for Player {player.Nickname} at Level {level}.";
    }
    
    public void EndMeltdown()
    {
        if (_meltdownTriggered)
        {
            _meltdownTriggered = false;
            RandomDelay = UnityEngine.Random.Range(MeltdownStart, MeltdownEnd);
            Timing.KillCoroutines(_metldownProcess);
            foreach (Room room in Room.List)
            {
                room.Color = new Color(1f, 1f, 1f);;
            }
            if (AudioPlayer.AudioPlayerByName.TryGetValue("GlobalAudioPlayer", out AudioPlayer ap))
            {
                ap.RemoveAllClips();
            }
        }
        else
        {
            Log.Info("Meltdown cannot be canceled, because theres no Meltdown!");
        }
    }
    
    public bool Refill(Player player)
    {
        if (IsReactorFueled(player)) return false;
        Item adrenalineItem = player.Items.FirstOrDefault(x => x.Type == ItemType.Adrenaline);
        if (adrenalineItem == null) return false;
        adrenalineItem.Destroy();
        Plugin.Singleton.Reactor.FuelReactor(player);
        return true;
    }

    #region Meltdown
    
    public void Meltdown(bool isRandom) => _metldownProcess = Timing.RunCoroutine(MeltdownProcess(RandomDelay));

    private IEnumerator<float> MeltdownProcess(float randomDelay)
    {
        if (!Warhead.IsDetonated)
        {
            _meltdownTriggered = true;
            if (!Plugin.Singleton.Config.UseCassieInsteadOfAudio)
            {
                AudioMeltdown();
                Cassie.MessageTranslated(
                    ".g1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .g1",
                    CassieTranslation, false, false);
            }
            else
            {
                Cassie.MessageTranslated(CassieMessage, CassieTranslation);
            }
            foreach (LightSourceObject Light in RoomScheme.gameObject.GetComponentsInChildren<LightSourceObject>())
            {
                Light.Light.Color = Plugin.Singleton.Config.MeltdownColor;
            }
            Warhead.Stop();
            Warhead.IsLocked = true;

            foreach (Room room in Room.List)
            {
                room.Color = Plugin.Singleton.Config.MeltdownColor;
            }
            
            foreach (Door Door in Door.List)
            {
                Door.IsOpen = true;
            }
            
            yield return Timing.WaitForSeconds(RandomDelay + 20f);

            if (AudioPlayer.AudioPlayerByName.TryGetValue("GlobalAudioPlayer", out AudioPlayer ap))
            {
                ap.RemoveAllClips();
            }

            if (!Round.IsEnded && !Round.IsLobby)
            {
                if (_meltdownTriggered)
                {
                    Warhead.IsLocked = false;
                    Warhead.Detonate();
                    Timing.KillCoroutines(_metldownProcess);
                    Plugin.Singleton.KillAreaCommand.AddDemonCore();
                    Plugin.Singleton.KillAreaCommand.StartCooldown();
                }
            }
        }
        else
        {
            Log.Info("Round has ended or is in lobby or Wahread is already Detonated, no explosion triggered!");
        }
    }

    #endregion

    public bool CanUseReactor(Player player, out double remainingTime)
    {
        if (Cooldowns.TryGetValue(player, out DateTime lastUse))
        {
            double elapsed = (DateTime.Now - lastUse).TotalSeconds;
            remainingTime = CommandCooldown - elapsed;
            return elapsed >= CommandCooldown;
        }

        remainingTime = 0;
        return true;
    }

    public void ApplyCooldown(Player player) => Cooldowns[player] = DateTime.Now;
    public void FuelReactor(Player player) => FueledReactors.Add(player);
    public void ConsumeReactorFuel(Player player) => FueledReactors.Remove(player);

    public bool IsReactorFueled(Player player)
    {
        return FueledReactors.Contains(player);
    }

    private void OnButtonInteracted(ButtonInteractedEventArgs ev)
    {
        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonStage1Name)
        {
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {ev.Player.Id} 1");
            return;
        }

        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonStage2Name)
        {
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {ev.Player.Id} 2");
            return;
        }

        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonStage3Name)
        {
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {ev.Player.Id} 3");
            return;
        }



        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonRefillName)
        {
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.FuelCommandName} {ev.Player.Id}");
            return;
        } 
    }
}