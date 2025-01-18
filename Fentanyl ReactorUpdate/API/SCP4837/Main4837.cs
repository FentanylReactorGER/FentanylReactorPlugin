using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.SCP4837
{
    public class Main4837
    {
        private static readonly Plugin Plugin = Plugin.Singleton;

        public Main4837()
        {
            RegisterEvents();
        }

        public void SubEvents()
        {
            RegisterEvents();
        }

        public void UnsEvents()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player.Role.Type == RoleType.Scp049)
            {
                ev.IsAllowed = false;
                Log.Info($"{ev.Player.Nickname} tried to interact with a door as SCP-049.");
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Target.Role.Type == RoleType.Scp049)
            {
                ev.Amount = 0;
                Log.Info($"{ev.Attacker.Nickname} tried to hurt SCP-049, but it was prevented.");
            }
        }
    }
}
