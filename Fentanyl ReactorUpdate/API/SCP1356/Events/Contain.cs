using System.Collections.Generic;
using System.Text.RegularExpressions;
using AdvancedMERTools;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using UnifiedEconomy.Helpers.Extension;
using UnityEngine;

namespace Fentanyl_ReactorUpdate.API.SCP1356.Events
{
    public class Contain
    {
        private static readonly Translation Translation = Plugin.Singleton.Translation;
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
        private readonly Dictionary<RoleTypeId, string> RoleTranslations = Translation.RoleTranslations;

        private readonly Dictionary<RoleTypeId, string> RoleTranslationsGer = Translation.RoleTranslationsCustomLanguage;

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
                    ev.Killer.ShowMeowHint(Translation.SCP1356CantContainRole);
                    ev.HealthObject.DoNotDestroyAfterDeath = true;
                    ev.HealthObject.Active = true;
                    ev.HealthObject.Health = 1650;
                    return;
                }
                if (ev.Killer.Role.Type != RoleTypeId.Tutorial)
                {
                    ContainSCP1356(ev.Killer, SCP1356);
                    Respawn.GrantTokens(ev.Killer.Role.Team.GetFaction(), 2);
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
            player.AddBalance(50f);
            string MoneyHint1356 = Translation.SCP1356MoneyHintContain.Replace("{0}", player.GetPlayerFromDB().Balance.ToString());
            player.ShowMeowHintMoney(MoneyHint1356);
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
                    : $"NATO_{player.UnitName[0]}";

                float unitNumber = 0;
                if (!string.IsNullOrEmpty(player.UnitName))
                {
                    Match match = Regex.Match(player.UnitName, @"\d+"); // Find the first number in the string
                    if (match.Success)
                    { 
                        unitNumber = float.Parse(match.Value); // Convert the number to a float
                    }
                }

                string ConCassieNtf = Translation.SCP1356CassieMessageContainNtf.Replace("{unitDesignation}", unitDesignation);
                string ConCassieNtfFinal = ConCassieNtf.Replace("{unitNumber}", unitNumber.ToString());
                return !string.IsNullOrEmpty(unitDesignation)
                    ? ConCassieNtfFinal
                    : "Mobile Task Force has contained SCP 1 3 5 6";
            }
            
            return RoleTranslations.TryGetValue(player.Role.Type, out string customTranslation)
                ? Translation.SCP1356CassieMessageContainOther.Replace("{customTranslation}", customTranslation)
                : $"SCP 1 3 5 6 contain succesfully containemnt reason unknown";
        }

        private string GetCassieTranslation(Player player)
        {
            if (player.Role.Team == Team.FoundationForces)
            {
                string unitDesignation = player.UnitName;
                return !string.IsNullOrEmpty(unitDesignation)
                    ? Translation.SCP1356CassieMessageTranslatedContainNtf.Replace("{unitDesignation}", unitDesignation)
                    : "Mobile Task Force hat SCP-1356 erfolgreich eingedämmt.";
            }

            return RoleTranslationsGer.TryGetValue(player.Role.Type, out string customTranslationGer)
                ? Translation.SCP1356CassieMessageTranslatedContainOther.Replace("{customTranslationGer}", customTranslationGer)
                : $"SCP-1356 Eindämmung erfolgreich durch {player.Role.Type.ToString()}.";
        }
    }
}