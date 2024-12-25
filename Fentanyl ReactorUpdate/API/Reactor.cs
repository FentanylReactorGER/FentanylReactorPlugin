using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.Events.EventArgs;
using MEC;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API;

public class Reactor
{
    public bool IsReplace => Plugin.Singleton.Config.ReplaceRoom;
    public RoomType RoomType => Plugin.Singleton.Config.RoomType;
    public int CommandCooldown => Plugin.Singleton.Config.CommandCooldown;
    public string SchematicName => Plugin.Singleton.Config.SchematicName;
    public float AutoStartIn => Plugin.Singleton.Config.MeltdownZeitStartRunde;
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

    public void RandomDelayGiver()
    {
        RandomDelay = UnityEngine.Random.Range(MeltdownStart, MeltdownEnd);
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
        //
    }

    public void UnSubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted -= OnButtonInteracted;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        //
    }

    private bool _meltdownTriggered = false;

    private void OnRoundStarted()
    {
        Room room = Room.Get(RoomType);
        if (room == null)
        {
            Log.Info("Room not found! Plugin is disabled.");
            Destroy();
            return;
        }

        if (Round.IsEnded)
        {
            Warhead.Stop();
            Warhead.IsLocked = false;
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

        if (AutoStartIn <= 0)
        {
            Log.Info("Meltdown autostart is disabled.");
            return;
        }

        if (_meltdownTriggered)
        {
            Log.Warn("Meltdown has already been triggered and will not occur again.");
            return;
        }

        Timing.CallDelayed(AutoStartIn, () =>
        {
            if (!_meltdownTriggered)
            {
                _meltdownTriggered = true;
                Meltdown(true);
            }
            else
            {
                Log.Warn("Meltdown attempt blocked as it has already been triggered.");
            }
        });
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
        AudioPlayer globalPlayer = AudioPlayer.Create("GlobalAudioPlayer");
        Speaker speaker = globalPlayer.AddSpeaker("GlobalSpeaker");
        globalPlayer.AddClip("Fentanyl Reactor Meltdown", Plugin.Singleton.Config.FentanylReactorAudioVolume, false, true);
        RandomDelayGiver();
        Warhead.Stop();
        Warhead.IsLocked = true;

        Cassie.MessageTranslated(CassieMessage, CassieTranslation);

        Color warningColor = new Color(0.150f, 0, 0.40f);
        foreach (Room room in Room.List)
        {
            room.Color = warningColor;
        }

        yield return Timing.WaitForSeconds(UnityEngine.Random.Range(MeltdownStart, MeltdownEnd) + 40f);


        Warhead.IsLocked = false;
        Warhead.Detonate();
        Timing.KillCoroutines(_metldownProcess);
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

    public void OnButtonInteracted(ButtonInteractedEventArgs ev)
    {
        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonStage1Name)
        {
            Log.Info($"{RoomScheme.Position.x} {RoomScheme.Position.y} {RoomScheme.Position.z}, Pos: {RoomScheme.Position}");
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {ev.Player.Id} {Plugin.Singleton.Config.T1ID}");
            return;
        }

        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonStage2Name)
        {
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {ev.Player.Id} {Plugin.Singleton.Config.T2ID}");
            return;
        }

        if (ev.Button.Base.name == Plugin.Singleton.Config.ButtonStage3Name)
        {
            ev.Button.Base.enabled = true;
            Server.ExecuteCommand($"/{Plugin.Singleton.Translation.CommandName} {ev.Player.Id} {Plugin.Singleton.Config.T3ID}");
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