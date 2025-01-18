using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using StartDamageCoroutine = Fentanyl_ReactorUpdate.API.SCP1356.Events;
using NorthwoodLib;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079.Map;
using UnityEngine;
using Cassie = Exiled.API.Features.Cassie;
using Map = Exiled.Events.Handlers.Map;
using Object = UnityEngine.Object;
using Player = Exiled.API.Features.Player;
using Server = Exiled.API.Features.Server;
using Warhead = Exiled.API.Features.Warhead;

namespace Fentanyl_ReactorUpdate.API.SCP1356;

public class MainDuck
{
    private CancellationTokenSource tokenSource;
    private CancellationToken token;

    private CustomItem DuckContainmentDevice { get; set; } = CustomItem.Get(2001);
    public Transform DuckScheme { get; set; }
    public Vector3 DuckPosition { get; set; }

    public void SubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned += OnSchematicSpawned;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        tokenSource = new CancellationTokenSource();
        token = tokenSource.Token;
    }

    public void UnsubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        tokenSource?.Cancel();
    }

    private void OnRoundStarted()
    {
        Timing.CallDelayed(1f, () => Plugin.Singleton.RadiationDamage.StartDamageCoroutine());
    }


    private void OnRoundEnded(RoundEndedEventArgs obj)
    {
        Plugin.Singleton.RadiationDamage.StopDamageCoroutine();
        tokenSource?.Cancel();
    }

    private void OnSchematicSpawned(MapEditorReborn.Events.EventArgs.SchematicSpawnedEventArgs ev)
    {
        Transform[] allChildren = ev.Schematic.gameObject.GetComponentsInChildren<Transform>();

        string targetName = "SCP1356RootObject";
        foreach (Transform childTransform in allChildren)
        {
            if (childTransform.gameObject.name == targetName)
            {
                DuckScheme = childTransform;
                DuckPosition = childTransform.GetChild(0).position;

                Log.Info($"DuckGay found and assigned: {DuckScheme.name} | {DuckPosition}");
            }
        }
    }
}