using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Fentanyl_ReactorUpdate.API;
using Fentanyl_ReactorUpdate.Configs;
using MEC;
using Mirror;
using UnityEngine;
using Random = System.Random;
using Fentanyl_ReactorUpdate.API.Extensions;
using UnityEngine.PlayerLoop;

namespace Fentanyl_ReactorUpdate;

public class Plugin : Plugin<Configs.Config, Configs.Translation>
{
    public override string Name => "Fentanyl Reactor";
    public override string Author => "SCP: Secret Fentanyl Server Team";
    public override Version Version => new Version(1, 3, 0);
    public override Version RequiredExiledVersion => new Version(9, 0, 1);
    public static Plugin Singleton = new Plugin();
    public static readonly Random Random = new Random();
    public Reactor Reactor { get; private set; }
        
    public override void OnEnabled()
    {
        AudioClipStorage.LoadClip(Path.Combine(Paths.Plugins, "FentReactorTest.ogg"), "Fentanyl Reactor");
        
        Singleton = this;
        CustomItem.RegisterItems();
        Reactor = new Reactor();
        UpdateChecker.RegisterEvents();
        UpdateSchematicChecker.RegisterEvents();
        base.OnEnabled();
    }

        
    public override void OnDisabled()
    {
        CustomRole.UnregisterRoles();
        Reactor.Destroy();
        Reactor = null;
        UpdateChecker.UnRegisterEvents();
        UpdateSchematicChecker.UnRegisterEvents();
        CustomItem.UnregisterItems();
        Singleton = null;
        base.OnDisabled();
    }
}