using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Fentanyl_ReactorUpdate.API.Extensions;
using InventorySystem.Items;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnifiedEconomy.Helpers.Extension;

namespace Fentanyl_ReactorUpdate.API.Classes;

public class MoneyEvents
{
    public class PlayerPickup
    {
        public Player Player { get; set; }
        public ItemType Item { get; set; } 

        public PlayerPickup(Player player, ItemType item)
        {
            Player = player;
            Item = item; 
        }

        
        
        public override bool Equals(object obj)
        {
            if (obj is not PlayerPickup other)
                return false;
            
            return Player == other.Player && Item == other.Item;
        }

        public override int GetHashCode()
        {
            int hashPlayer = Player != null ? Player.GetHashCode() : 0;
            int hashItem = Item != null ? Item.GetHashCode() : 0;
            return hashPlayer ^ hashItem;  // XOR for combining hash codes
        }
    }
    
    public class GenPlayers
    {
        public Player Player { get; set; }
        public Generator Gen { get; set; } 

        public GenPlayers(Player player, Generator Gen)
        {
            Player = player;
            Gen = Gen; 
        }

        
        
        public override bool Equals(object obj)
        {
            if (obj is not GenPlayers other)
                return false;
            
            return Player == other.Player && Gen == other.Gen;
        }
        public override int GetHashCode()
        {
            int hashPlayer = Player != null ? Player.GetHashCode() : 0;
            int hashItem = Gen != null ? Gen.GetHashCode() : 0;
            return hashPlayer ^ hashItem;  // XOR for combining hash codes
        }
    }
    
    private List<PlayerPickup> PickupedPlayers = new List<PlayerPickup>();
    private List<GenPlayers> GenPlayersList = new List<GenPlayers>();
    private Dictionary<Player, Player> CuffPlayerList = new Dictionary<Player, Player>();

    private List<ItemType> ItemTypes = new List<ItemType>
    {
        ItemType.KeycardO5,
        ItemType.MicroHID,
        ItemType.Jailbird,
        ItemType.ParticleDisruptor
    };
    
    private List<uint> CustomItemTypes = new List<uint>
    {
        90,
        1113,
        1112,
        6912,
        1488,
    };

    public void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.Escaped += OnEscaped;
        Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
        Exiled.Events.Handlers.Server.RoundStarted += SpawningItem;
        Exiled.Events.Handlers.Player.UnlockingGenerator += OnGen;
        Exiled.Events.Handlers.Server.RoundStarted += StartCheck;
    }

    public void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= SpawningItem;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.Escaped -= OnEscaped;
        Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        Exiled.Events.Handlers.Player.UnlockingGenerator -= OnGen;
        Exiled.Events.Handlers.Server.RoundStarted -= StartCheck;
    }

    private void StartCheck()
    {
        CuffPlayerList.Clear();
        Timing.RunCoroutine(CheckExploiters());
    }
    
    private void OnGen(UnlockingGeneratorEventArgs ev)
    {
        if (!GenPlayersList.Contains(new GenPlayers(ev.Player, ev.Generator)))
        {
            GenPlayersList.Add(new GenPlayers(ev.Player, ev.Generator));
            ev.Player.ShowMeowHint($"Für diesen Generator hast du 15 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} Erhalten! \n Dein Kontostand: {ev.Player.GetPlayerFromDB().Balance} SOL");
            ev.Player.AddBalance(15);
        }

        return;
    }

    private Dictionary<Player, float> PlayerLastBalance = new Dictionary<Player, float>();
    private Dictionary<Player, float> PlayerBalanceGain = new Dictionary<Player, float>();

    private IEnumerator<float> CheckExploiters()
    {
        while (!Round.IsEnded)
        {
            foreach (Player player in Player.List)
            {
                float currentBalance = player.GetPlayerFromDB().Balance;
                
                if (!PlayerLastBalance.ContainsKey(player))
                {
                    PlayerLastBalance[player] = currentBalance;
                    PlayerBalanceGain[player] = 0;
                }
                
                float balanceChange = currentBalance - PlayerLastBalance[player];
                if (balanceChange > 0)
                {
                    PlayerBalanceGain[player] += balanceChange;
                }
                
                PlayerLastBalance[player] = currentBalance;
                
                if (PlayerBalanceGain[player] > 300)
                {
                    foreach (Player p in Player.List)
                    {
                        if (p.RemoteAdminAccess)
                        {
                            p.ShowMeowHint($"<color=yellow>⚠️</color> {player.Nickname} (ID: {player.UserId}) hat über 300 {Plugin.Singleton.WebSocketServer.GetCustomMessage(p.UserId)} in 5 Minuten erzeugt! \n <color=red>[Exploit Alarm]</color> \n Nur für Admins sichtbar!");
                        }
                    }
                    Log.Info($"Player {player.Nickname} (ID: {player.UserId}) gained over 200 currency within 5 minutes.");
                    PlayerBalanceGain[player] = 0; 
                }
            }
            
            yield return Timing.WaitForSeconds(1);
        }
        PlayerLastBalance.Clear();
        PlayerBalanceGain.Clear();
        
        yield break;
    }
    
    private void OnEscaped(EscapedEventArgs ev)
    {
        if (ev.EscapeScenario != EscapeScenario.CuffedScientist && ev.EscapeScenario != EscapeScenario.CuffedClassD)
        {
            ev.Player.ShowMeowHint($"Für diesen Escape hast du 15 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} Erhalten! \n Dein Kontostand: {ev.Player.GetPlayerFromDB().Balance} SOL");
            ev.Player.AddBalance(15);
        }
    }
    
    private void OnHandcuffing(HandcuffingEventArgs ev)
    {
        if (CuffPlayerList.ContainsKey(ev.Player) || CuffPlayerList.ContainsValue(ev.Target))
        {
            return;
        }

        if (Round.IsStarted)
        {
            ev.Player.ShowMeowHint($"Für diesen Cuff hast du 5 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} erhalten! \n Dein Kontostand: {ev.Player.GetPlayerFromDB().Balance} SOL");
            ev.Player.AddBalance(5);

            CuffPlayerList.Add(ev.Player, ev.Target);
        }
    }
    
    private void SpawningItem()
    {
        GenPlayersList.Clear();
        PickupedPlayers.Clear();
    }

    private void OnDied(DiedEventArgs ev)
    {
        if (Round.IsStarted)
        {
            ev.Attacker.ShowMeowHint($"Für dieses Kill von {ev.Player.DisplayNickname} hast du 10 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} Erhalten! \n Dein Kontostand: {ev.Attacker.GetPlayerFromDB().Balance} SOL");
            ev.Attacker.AddBalance(10);
        }
    }
    
    private void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if (PickupedPlayers.Contains(new PlayerPickup(ev.Player, ev.Pickup.Base.ItemId.TypeId)))
        {
            Log.Info($"Player {ev.Player.Nickname} has already picked up this item.");
            return;
        }
        if (ItemTypes.Contains(ev.Pickup.Base.ItemId.TypeId))
        {
            ev.Player.ShowMeowHint($"Für dieses Item hast du 35 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} erhalten! \n Dein Kontostand: {ev.Player.GetPlayerFromDB().Balance} SOL");
            ev.Player.AddBalance(35);
            PickupedPlayers.Add(new PlayerPickup(ev.Player, ev.Pickup.Base.ItemId.TypeId));
        }
        else if (CustomItem.TryGet(ev.Pickup, out CustomItem customItem))
        {
            if (customItem?.Id != null)
            {
                if (CustomItemTypes.Contains(customItem.Id))
                {
                    ev.Player.ShowMeowHint($"Für dieses Custom Item hast du 25 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} erhalten! \n Dein Kontostand: {ev.Player.GetPlayerFromDB().Balance} SOL");
                    ev.Player.AddBalance(25);
                    PickupedPlayers.Add(new PlayerPickup(ev.Player, ev.Pickup.Base.ItemId.TypeId));
                }
                else
                {
                    ev.Player.ShowMeowHint($"Für dieses Custom Item hast du 5 {Plugin.Singleton.WebSocketServer.GetCustomMessage(ev.Player.UserId)} erhalten! \n Dein Kontostand: {ev.Player.GetPlayerFromDB().Balance} SOL");
                    ev.Player.AddBalance(5);
                    PickupedPlayers.Add(new PlayerPickup(ev.Player, ev.Pickup.Base.ItemId.TypeId));
                }
            }
        }
    }
}