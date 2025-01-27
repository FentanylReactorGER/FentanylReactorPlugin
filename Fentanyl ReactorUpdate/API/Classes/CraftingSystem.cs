using Fentanyl_ReactorUpdate.API.Extensions;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    using Exiled.API.Features;
    using MEC;
    using Exiled.Events.EventArgs.Player;
    using UnityEngine;
    using Vector3 = UnityEngine.Vector3;
    using Mirror;
    using Exiled.API.Features.Pickups;
    using System.Linq;
    using Exiled.API.Enums;
    using System.Collections.Generic;
    using System;

    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using ServerHandlers = Exiled.Events.Handlers.Server;
    using HintServiceMeow.UI.Utilities;

    public class CraftingSystem
    {

        private string CraftingHintColor = Plugin.Singleton.Config.CraftingHintColor.ToHex();
        public ushort CraftingSystemId;
        public GameObject craftingSystem;
        public Pickup item;

        public void Initialize()
        {
            ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;

            ServerHandlers.RoundStarted += OnRoundStart;

            PlayerHandlers.Verified += VerifiedEvent;

            PlayerHandlers.PickingUpItem += Crafting;
        }

        public void Uninitialize()
        {
            ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;

            ServerHandlers.RoundStarted -= OnRoundStart;

            PlayerHandlers.Verified -= VerifiedEvent;

            PlayerHandlers.PickingUpItem -= Crafting;
        }

        //Dictionary for recipes
        public Dictionary<string, ItemType> Recipes { get; private set; } = new();
        public List<Exiled.API.Features.Player> players = Exiled.API.Features.Player.List.ToList();

        //Item in mid
        public static Dictionary<bool, Pickup> puttedItem = new Dictionary<bool, Pickup>();
        public Pickup itemMid;
        private bool combine = false;
        private ushort CurrentItem;
        public string InsideItem1 = " ";
        public string InsideItem2 = " ";
        public string CombineInsideItem = " ";

        //Player var
        public float Strengh = 1f;
        System.Random random = new System.Random();

        //List of rooms
        List<RoomType> rooms = new List<RoomType>();



        public void OnWaitingForPlayers()
        {
            try
            {
                Recipes.Clear();
                rooms.Clear();
                //Load all recipes

                #region Recipes

                foreach (var recipe in Plugin.Singleton.Config.Recipes)
                {
                    if (Enum.TryParse(recipe.Value, out ItemType outputItem))
                    {
                        Recipes.Add(recipe.Key, outputItem);
                    }
                    else
                    {
                        Log.Warn($"Invalid ItemType '{recipe.Value}' for recipe '{recipe.Key}'. Skipping.");
                    }
                }

                #endregion

                #region List every Room

                rooms.Add(RoomType.EzCafeteria);
                rooms.Add(RoomType.EzCheckpointHallwayA);
                rooms.Add(RoomType.EzConference);
                rooms.Add(RoomType.EzCrossing);
                rooms.Add(RoomType.EzCurve);
                rooms.Add(RoomType.EzGateA);
                rooms.Add(RoomType.EzGateB);
                rooms.Add(RoomType.EzIntercom);
                rooms.Add(RoomType.EzPcs);
                rooms.Add(RoomType.EzTCross);
                rooms.Add(RoomType.EzUpstairsPcs);
                rooms.Add(RoomType.Hcz049);
                rooms.Add(RoomType.Hcz079);
                rooms.Add(RoomType.Hcz096);
                rooms.Add(RoomType.Hcz106);
                rooms.Add(RoomType.Hcz939);
                rooms.Add(RoomType.HczArmory);
                rooms.Add(RoomType.HczCrossing);
                rooms.Add(RoomType.HczCurve);
                rooms.Add(RoomType.HczElevatorA);
                rooms.Add(RoomType.HczElevatorB);
                rooms.Add(RoomType.HczEzCheckpointA);
                rooms.Add(RoomType.HczEzCheckpointB);
                rooms.Add(RoomType.HczHid);
                rooms.Add(RoomType.HczNuke);
                rooms.Add(RoomType.HczStraight);
                rooms.Add(RoomType.EzTCross);
                rooms.Add(RoomType.Lcz330);
                rooms.Add(RoomType.Lcz914);
                rooms.Add(RoomType.LczAirlock);
                rooms.Add(RoomType.LczArmory);
                rooms.Add(RoomType.LczCheckpointA);
                rooms.Add(RoomType.LczCheckpointB);
                rooms.Add(RoomType.LczClassDSpawn);
                rooms.Add(RoomType.LczCrossing);
                rooms.Add(RoomType.LczCurve);
                rooms.Add(RoomType.LczGlassBox);
                rooms.Add(RoomType.LczPlants);
                rooms.Add(RoomType.LczToilets);
                rooms.Add(RoomType.LczTCross);
                rooms.Add(RoomType.Pocket);
                rooms.Add(RoomType.Surface);

                #endregion
            }
            catch (Exception e)
            {
                Log.Info($"OnWaitingForPlayers Error\nError: {e.Message}");
            }
        }

        public void VerifiedEvent(VerifiedEventArgs ev)
        {

        }

        private readonly HashSet<Player> _playersInCrafting = new();

        private IEnumerator<float> CheckForPlayers(Pickup item)
        {
            while (Round.InProgress && !Warhead.IsDetonated)
            {
                foreach (Player player in Player.List.Where(p =>
                             p.IsAlive && Vector3.Distance(p.Position, item.Position) < 14))
                {
                    if (!_playersInCrafting.Contains(player))
                    {
                        _playersInCrafting.Add(player);
                        string enterHint = Plugin.Singleton.Translation.EnterCraftingMenu.Replace("{PlayerName}", player.DisplayNickname);
                        player.ShowMeowHintDur(enterHint, 15);
                    }
                }

                foreach (Player player in _playersInCrafting.ToList())
                {
                    if (!player.IsAlive || Vector3.Distance(player.Position, item.Position) >= 14)
                    {
                        _playersInCrafting.Remove(player);
                    }
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }

        public void OnRoundStart()
        {
            try
            {
                //Clear mid item
                puttedItem.Clear();
                InsideItem1 = " ";
                InsideItem2 = " ";

                //Create pickup
                item = Exiled.API.Features.Items.Item.Create(ItemType.SCP500).CreatePickup(UnityEngine.Vector3.zero);
                Timing.RunCoroutine(CheckForPlayers(item));

                //Convert to gameObject
                craftingSystem = item.GameObject;
                CraftingSystemId = item.Serial;
                NetworkServer.UnSpawn(craftingSystem);

                //Declare properties of craftingSystem
                craftingSystem.transform.parent = Room.List.First(x => x.Type == RoomType.Hcz096).transform;
                craftingSystem.GetComponent<Rigidbody>().useGravity = false;
                craftingSystem.GetComponent<Rigidbody>().drag = 0f;
                craftingSystem.GetComponent<Rigidbody>().freezeRotation = true;
                craftingSystem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                craftingSystem.SetActive(true);
                craftingSystem.transform.localPosition = new Vector3(0.85f, 1.5f, -0.035f);
                craftingSystem.transform.localRotation = UnityEngine.Quaternion.Euler(0, 0, 90);
                craftingSystem.transform.localScale = new UnityEngine.Vector3(10, 10, 10);

                NetworkServer.Spawn(craftingSystem);
            }
            catch (Exception e)
            {
                Log.Info($"OnRoundStart Error\nError: {e.Message}");
            }
        }

        public void SpawnCurrentItem(ItemType itemType, GameObject craftingSystem, bool Merge = false)
        {
            try
            {
                //Create mid pickup
                itemMid = Exiled.API.Features.Items.Item.Create(itemType).CreatePickup(UnityEngine.Vector3.zero);

                //Convert to gameObject
                GameObject currentItem = itemMid.GameObject;
                CurrentItem = itemMid.Serial;
                NetworkServer.UnSpawn(currentItem);

                //Declare properties of craftingSystem
                currentItem.transform.parent = craftingSystem.transform;
                currentItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                currentItem.GetComponent<Rigidbody>().isKinematic = true;
                currentItem.GetComponent<Rigidbody>().useGravity = false;
                currentItem.GetComponent<Rigidbody>().drag = 0f;
                currentItem.GetComponent<Rigidbody>().freezeRotation = true;
                currentItem.SetActive(true);

                if (itemType == ItemType.MicroHID)
                {

                    currentItem.transform.localPosition = new Vector3(0f, 0.12f, 0f);
                    currentItem.transform.localRotation = UnityEngine.Quaternion.Euler(-90, 90, 0);
                    currentItem.transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);

                }
                else
                {
                    currentItem.transform.localPosition = new Vector3(0f, 0.11f, 0f);
                    currentItem.transform.localRotation = UnityEngine.Quaternion.Euler(0, 0, 0);
                    currentItem.transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
                }

                if (!Merge)
                {
                    puttedItem.Add(true, itemMid);
                }

                NetworkServer.Spawn(currentItem);
            }
            catch (Exception e)
            {
                Log.Info($"SpawnCurrentItem Error\nError: {e.Message}");
            }
        }

        /*public void PlayerShoot(PlayerShotWeaponEvent ev)
        {

        }*/

        public void Crafting(PickingUpItemEventArgs ev)
        {
            try
            {
                // Überprüfen, ob das aufgenommene Item zum Crafting-System gehört
                if (CraftingSystemId == ev.Pickup.Serial)
                {
                    if (ev.Player.CurrentItem != null)
                    {
                        // Überprüfen, ob das aktuelle Item nicht vercraftet werden kann
                        if (ev.Player.CurrentItem.Type == ItemType.SCP018 ||
                            ev.Player.CurrentItem.Type == ItemType.MicroHID ||
                            ev.Player.CurrentItem.Type == ItemType.Jailbird ||
                            ev.Player.CurrentItem.Type == ItemType.SCP1576 ||
                            ev.Player.CurrentItem.Type == ItemType.SCP268 ||
                            ev.Player.CurrentItem.Type == ItemType.ParticleDisruptor)
                        {
                            string itemTypeGermanTranslation =
                                Plugin.Singleton.Enmm.ItemTranslations.TryGetValue(ev.Player.CurrentItem.Type,
                                    out string translation)
                                    ? translation
                                    : "Unbekannter Gegenstand";
                            var message =
                                $"Du kannst <color={CraftingHintColor}><b>{itemTypeGermanTranslation}</b></color> nicht vercraften.";
                            ev.Player.ShowMeowHint(message);
                            ev.IsAllowed = false;
                            return;
                        }

                        // Wenn kein Item im Crafting-System ist, füge das erste Item hinzu
                        if (InsideItem1 == " ")
                        {
                            InsideItem1 = ev.Player.CurrentItem.Type.ToString();

                            // Spawn des ersten Items
                            SpawnCurrentItem(ev.Player.CurrentItem.Type, craftingSystem);
                            ev.Player.RemoveItem(ev.Player.CurrentItem);
                        }
                        else
                        {
                            // Das erste Item ist schon drin, zerstöre das vorherige und speichere das zweite
                            puttedItem[true].Destroy();
                            puttedItem.Clear();

                            InsideItem2 = ev.Player.CurrentItem.Type.ToString();
                            ev.Player.RemoveItem(ev.Player.CurrentItem);
                            CombineInsideItem = InsideItem1 + InsideItem2;

                            // Suche nach dem Rezept
                            foreach (var recipe in Recipes)
                            {
                                if (CombineInsideItem == recipe.Key)
                                {
                                    string insideItem1Translation =
                                        Plugin.Singleton.Enmm.ItemTranslations.TryGetValue(
                                            (ItemType)Enum.Parse(typeof(ItemType), InsideItem1),
                                            out string translation1)
                                            ? translation1
                                            : "Unbekannter Gegenstand";
                                    string insideItem2Translation =
                                        Plugin.Singleton.Enmm.ItemTranslations.TryGetValue(
                                            (ItemType)Enum.Parse(typeof(ItemType), InsideItem2),
                                            out string translation2)
                                            ? translation2
                                            : "Unbekannter Gegenstand";

                                    if (CombineInsideItem == "GrenadeHEKeycardChaosInsurgency" ||
                                        CombineInsideItem == "KeycardChaosInsurgencyGrenadeHE")
                                    {
                                        ev.Player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
                                        var message =
                                            $"Du hast <color={CraftingHintColor}><b>{insideItem1Translation}</b></color> mit <color={CraftingHintColor}><b>{insideItem2Translation}</b></color> kombiniert.";
                                        ev.Player.ShowMeowHint(message);
                                        combine = true;
                                        break;
                                    }
                                    else if (CombineInsideItem == "RadioPainkillers" ||
                                             CombineInsideItem == "PainkillersRadio")
                                    {
                                        // Teleportiere den Spieler zu einem zufälligen Raum
                                        var tempRoom = rooms[random.Next(rooms.Count)];
                                        var message =
                                            $"Du wirst in <color={CraftingHintColor}><b>3</b></color> Sekunden zum Raum teleportiert <color={CraftingHintColor}><b>{tempRoom}</b></color>.";
                                        ev.Player.ShowMeowHint(message);
                                        ev.Player.EnableEffect(EffectType.Ensnared, 2, 5);
                                        Timing.CallDelayed(1, () =>
                                        {
                                            message =
                                                $"Du wirst in <color={CraftingHintColor}><b>2</b></color> Sekunden.";
                                            ev.Player.ShowMeowHintDur(message, 1);
                                            Timing.CallDelayed(1, () =>
                                            {
                                                message =
                                                    $"Du wirst in <color={CraftingHintColor}><b>1</b></color> Sekunden.";
                                                ev.Player.ShowMeowHintDur(message, 1);
                                                Timing.CallDelayed(1, () =>
                                                {
                                                    message = $"Du wurdest in den Raum teleportiert";
                                                    ev.Player.ShowMeowHintDur(message, 1);
                                                    ev.Player.Teleport(tempRoom);
                                                    if (tempRoom == RoomType.Pocket)
                                                    {
                                                        ev.Player.EnableEffect(EffectType.Corroding, 15);
                                                    }
                                                });
                                            });
                                        });
                                        combine = true;
                                        break;
                                    }
                                    else
                                    {
                                        string resultItemTranslation =
                                            Plugin.Singleton.Enmm.ItemTranslations.TryGetValue(
                                                (ItemType)Enum.Parse(typeof(ItemType), recipe.Value.ToString(), true),  // "true" for case-insensitive matching
                                                out string resultTranslation)
                                                ? resultTranslation
                                                : "Unbekanntes Ergebnis";


                                        SpawnCurrentItem(recipe.Value, craftingSystem, true);
                                        var message =
                                            $"Du hast <color={CraftingHintColor}><b>{insideItem1Translation}</b></color> mit <color={CraftingHintColor}><b>{insideItem2Translation}</b></color> kombiniert. Ergebnis: <color={CraftingHintColor}><b>{resultItemTranslation}</b></color>.";
                                        ev.Player.ShowMeowHint(message);
                                        combine = true;
                                        break;
                                    }
                                }
                            }

                            // Wenn keine Kombination gefunden wurde, zeige eine Nachricht an
                            if (!combine)
                            {
                                string insideItem1Translation =
                                    Plugin.Singleton.Enmm.ItemTranslations.TryGetValue(
                                        (ItemType)Enum.Parse(typeof(ItemType), InsideItem1), out string translation1)
                                        ? translation1
                                        : "Unbekannter Gegenstand";
                                string insideItem2Translation =
                                    Plugin.Singleton.Enmm.ItemTranslations.TryGetValue(
                                        (ItemType)Enum.Parse(typeof(ItemType), InsideItem2), out string translation2)
                                        ? translation2
                                        : "Unbekannter Gegenstand";

                                var message =
                                    $"<color={CraftingHintColor}><b>{insideItem1Translation}</b></color> ist mit <color={CraftingHintColor}><b>{insideItem2Translation}</b></color> nicht kombinierbar.\nRezepte findest du auf unserem Discord.";
                                ev.Player.ShowMeowHint(message);
                            }

                            combine = false;
                            InsideItem1 = " ";
                            InsideItem2 = " ";
                        }
                    }
                    else
                    {
                        // Wenn kein Item in der Hand ist, zeige eine Nachricht an
                        var message =
                            $"Nimm ein <color={CraftingHintColor}><b>Item</b></color> in die Hand, um es zu vercraften.\nRezepte findest du auf unserem Discord!";
                        ev.Player.ShowMeowHint(message);
                    }

                    ev.IsAllowed = false;
                }
                else if (CurrentItem == ev.Pickup.Serial)
                {
                    // Rücksetzen des aufgenommenen Items
                    ev.Pickup.GameObject.transform.localScale = new Vector3(1, 1, 1);
                    puttedItem.Clear();
                    InsideItem1 = " ";
                    InsideItem2 = " ";
                }
            }
            catch (Exception e)
            {
                puttedItem.Clear();
                ev.IsAllowed = false;
                Log.Info($"Crafting-System Error\nError: {e.Message}");
            }
        }
    }
}