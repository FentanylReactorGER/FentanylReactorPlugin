using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using UnifiedEconomy.Helpers.Extension;
using UnityEngine;

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
        Exiled.Events.Handlers.Player.Joined += Joined;
        tokenSource = new CancellationTokenSource();
        token = tokenSource.Token;
    }

    public void UnsubEvents()
    {
        MapEditorReborn.Events.Handlers.Schematic.SchematicSpawned -= OnSchematicSpawned;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.Joined -= Joined;
        tokenSource?.Cancel();
    }

    private void Joined(JoinedEventArgs ev)
    {
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