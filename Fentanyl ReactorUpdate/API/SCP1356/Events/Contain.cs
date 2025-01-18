using System.Collections.Generic;
using AdvancedMERTools;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.SCP1356.Events
{
    public class Contain
    {
        public void SubEvents()
        {
            EventHandler.HealthObjectDead += OnHealthObjectDead;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }
        public void UnsubEvents()
        {
            EventHandler.HealthObjectDead -= OnHealthObjectDead;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }
        private HealthObject SCP1356HealthObject { get; set; } 
        private SchematicObject SCP1356 { get; set; } 
        private readonly Dictionary<RoleTypeId, string> RoleTranslations = new()
        {
            { RoleTypeId.ClassD, "Class D Personnel" },
            { RoleTypeId.Scientist, "Scientist Personnel" },
            { RoleTypeId.ChaosConscript, "Chaos Insurgency" },
            { RoleTypeId.ChaosMarauder, "Chaos Insurgency" },
            { RoleTypeId.ChaosRepressor, "Chaos Insurgency" },
            { RoleTypeId.ChaosRifleman, "Chaos Insurgency" }
        };
        
        private readonly Dictionary<RoleTypeId, string> RoleTranslationsGer = new()
        {
            { RoleTypeId.ClassD, "Klasse-D Personal" },
            { RoleTypeId.Scientist, "Wissenschaftliches Personal" },
            { RoleTypeId.ChaosConscript, "Chaos Insurgency" },
            { RoleTypeId.ChaosMarauder, "Chaos Insurgency" },
            { RoleTypeId.ChaosRepressor, "Chaos Insurgency" },
            { RoleTypeId.ChaosRifleman, "Chaos Insurgency" }
        };

        private void OnRoundStarted()
        {
            Timing.CallDelayed(2f,
                () =>
                {
                    SCP1356 = Plugin.Singleton.RadiationDamage.SCP1356;
                    foreach (Component component in SCP1356.GetComponentsInChildren<Component>())
                    {
                        if (component.GetComponent<HealthObject>())
                        {
                            SCP1356HealthObject = component.GetComponent<HealthObject>();
                        }
                    }
                });
        }
        
        private void OnHealthObjectDead(HealthObjectDeadEventArgs ev)
        {
            if (ev.HealthObject.ObjectId == SCP1356HealthObject.Base.ObjectId)
            {
                if (ev.Killer.Role.Type == RoleTypeId.Tutorial)
                {
                    ev.Killer.ShowMeowHint("Als D.U.C.K kannst du SCP-1356 nicht Eindämmen!");
                    ev.HealthObject.DoNotDestroyAfterDeath = true;
                    ev.HealthObject.Active = true;
                    ev.HealthObject.Health = 1650;
                    return;
                }
                if (ev.Killer.Role.Type != RoleTypeId.Tutorial)
                {
                    ContainSCP1356(ev.Killer, SCP1356);
                    ev.HealthObject.DoNotDestroyAfterDeath = false;
                    ev.HealthObject.Active = false;
                }
            }
            if (ev.HealthObject.ObjectId == "417 3")
            {
                Log.Info("SCP-1356 Glass offen");
                Plugin.Singleton.RadiationDamage.IsSCP1356GlassOpen = true;
            }
        }

        
        public void ContainSCP1356(Player player, SchematicObject SCP1356)
        {
                SCP1356.Destroy();
                string cassieMessage = GetCassieMessage(player);
                string cassieTranslation = GetCassieTranslation(player);
                
                Cassie.MessageTranslated(cassieMessage, cassieTranslation);
        }
        
        private string GetCassieMessage(Player player)
        {
            if (player.Role.Team == Team.FoundationForces)
            {
                string unitDesignation = string.IsNullOrEmpty(player.UnitName)
                    ? null
                    : $"NATO_{player.UnitName[0]} . {player.UnitName.Substring(1)}";

                return !string.IsNullOrEmpty(unitDesignation)
                    ? $"SCP 1 3 5 6 contained successfully . Containmentunit {unitDesignation}"
                    : "Mobile Task Force has contained SCP 1 3 5 6";
            }

            return RoleTranslations.TryGetValue(player.Role.Type, out string customTranslation)
                ? $"SCP 1 3 5 6 contained successfully by {customTranslation}"
                : $"{player.Role.Type.ToString()} has contained SCP 1 3 5 6";
        }

        private string GetCassieTranslation(Player player)
        {
            if (player.Role.Team == Team.FoundationForces)
            {
                string unitDesignation = player.UnitName;
                return !string.IsNullOrEmpty(unitDesignation)
                    ? $"SCP-1356 Eindämmung erfolgreich. Eindämmungseinheit: {unitDesignation}."
                    : "Mobile Task Force hat SCP-1356 erfolgreich eingedämmt.";
            }

            return RoleTranslationsGer.TryGetValue(player.Role.Type, out string customTranslationGer)
                ? $"SCP-1356 Eindämmung erfolgreich durch {customTranslationGer}."
                : $"SCP-1356 Eindämmung erfolgreich durch {player.Role.Type.ToString()}.";
        }
    }
}