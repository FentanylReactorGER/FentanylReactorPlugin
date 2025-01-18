using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Fentanyl_ReactorUpdate.API.Extensions;
using Fentanyl_ReactorUpdate.Configs;
using MEC;
using PlayerRoles;
using Object = UnityEngine.Object;

namespace Fentanyl_ReactorUpdate.API.SCP1356.CustomItems1356;

[CustomItem(ItemType.KeycardChaosInsurgency)]
public class RCDevice : CustomItem
{
    private static readonly Config Config = Plugin.Singleton.Config;
    private static readonly Translation Translation = Plugin.Singleton.Translation;
    public override string Name { get; set; } = "SCP-1356 Eindämmungsgerät";
    public override string Description { get; set; } = "Wird von der D.U.C.K genutzt, um SCP-1356 einzudämmen.";
    public override float Weight { get; set; } = Config.T1Weight;
    public override uint Id { get; set; } = 1488;
    public override SpawnProperties SpawnProperties { get; set; }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted += OnButtonInteracted;
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        MapEditorReborn.Events.Handlers.Schematic.ButtonInteracted -= OnButtonInteracted;
    }

    private void OnButtonInteracted(MapEditorReborn.Events.EventArgs.ButtonInteractedEventArgs ev)
    {
        if (ev.Button.Base.name == "SCP1356Button")
        {
            ev.Button.Base.enabled = true;
            if (ev.Player.Role.Type == RoleTypeId.Tutorial)
            {
                if (Check(ev.Player.CurrentItem))
                {
                    ev.Player.ShowHint("Du hast SCP-1356 befreit und eine D.U.C.K Spawnwelle erzwungen! ");
                    Server.ExecuteCommand("/forcesh");
                    foreach (Player player in Player.List)
                    {
                        if (player != ev.Player)
                        {
                            player.ShowMeowHint("D.U.C.K Spawnwelle wurde erzwungen");
                        }
                    }

                    Plugin.Singleton.SCP1356Breach = false;
                    Cassie.MessageTranslated(
                        "pitch_0.7 BELL_START .g4 .g6 pitch_1 SCP 1 3 5 6 got pitch_0.9 captured pitch_1 by D U C K intruders . . continue with pitch_0.9 caution pitch_1 pitch_0.7 .g6 .g4 BELL_END pitch_1",
                        "<color=yellow>SCP-1356</color> wurde von der <color=yellow>D.U.C.K</color> beschlagnahmt. Fahren sie mit <color=red>Vorsicht</color> fort!");
                    ev.Schematic.Destroy();
                    Plugin.Singleton.RadiationDamage.IsSCP1356Captured = true;
                    // Timing.RunCoroutine(GrabScheme(token, ev.Player, SCP1356.GetComponentInChildren<Rigidbody>()));
                }

                if (ev.Player.CurrentItem is null ||
                    !Check(ev.Player.CurrentItem))
                {
                    ev.Player.ShowHint("Du musst dein Eindämmungsgerät in der Hand halten, um SCP-1356 zu fangen!");
                }
            }
            else if (ev.Player.Role.Type != RoleTypeId.Tutorial)
            {
                ev.Player.ShowMeowHint(
                    "Du bist kein Mitglied der D.U.C.K und kannst SCP-1356 nicht fangen! \n Wie zu Hölle lebst du noch?");
            }
        }

        return;
    }
}