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
using Fentanyl_ReactorUpdate.API.SCP4837;
using Fentanyl_ReactorUpdate.Configs;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using SSMenuSystem.Features;
using UnityEngine;
using UserSettings.ServerSpecific;
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
        Exiled.Events.Handlers.Player.TogglingNoClip += OnButtonInteracted;
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Player.TogglingNoClip -= OnButtonInteracted;
    }

    private void OnButtonInteracted(TogglingNoClipEventArgs ev)
    {
        SSTwoButtonsSetting UseNoclipKey = ev.Player.ReferenceHub.GetParameter<SCP4837InteractionMenu, SSTwoButtonsSetting>(417);
        if (UseNoclipKey.SyncIsA)
        {
            Plugin.Singleton.SCP4837InteractionMenu.Check1356Containment(ev.Player);
        }
    }
}