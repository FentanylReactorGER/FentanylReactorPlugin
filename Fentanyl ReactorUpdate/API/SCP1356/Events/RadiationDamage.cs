using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Features;
using Fentanyl_ReactorUpdate.API.Extensions;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using MEC;
using NorthwoodLib;
using PlayerRoles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fentanyl_ReactorUpdate.API.SCP1356.Events
{
    public class RadiationDamage
    {
        private EffectType effectone = Plugin.Singleton.Config.EffectOne;
        private EffectType effecttwo = Plugin.Singleton.Config.EffectTwo;
        private EffectType effectthree = Plugin.Singleton.Config.EffectThree;
        private int effectDuration = Plugin.Singleton.Config.EffectDuration;
        public SchematicObject SCP1356 { get; set; }
        public bool IsSCP1356Captured { get; set; } = false;
        public bool IsSCP1356GlassOpen { get; set; } = false;
        public Transform scp1356RootObject { get; set; }
        private Vector3 scp1356RootPosition { get; set; }
        private readonly List<Player> _playersInSCPRange = new List<Player>();
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        public void StartDamageCoroutine()
        {
            string SchematicName = "GameObject";
            scp1356RootPosition = Plugin.Singleton.MainDuck.DuckPosition;
            scp1356RootObject = Plugin.Singleton.MainDuck.DuckScheme;
            SCP1356 = ObjectSpawner.SpawnSchematic(SchematicName, scp1356RootObject.position,
                scp1356RootObject.rotation, scp1356RootObject.localScale,
                MapUtils.GetSchematicDataByName(SchematicName), true);
            Log.Info($"{SCP1356.Position} | {SCP1356.Rotation} | {SCP1356.Scale}");
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            Timing.RunCoroutine(PlayerEscapeCheck(token), Segment.FixedUpdate);
        }

        public void StopDamageCoroutine()
        {
            IsSCP1356GlassOpen = false;
            IsSCP1356Captured = false;
            _playersInArea?.Clear();
            _DUCKMembers?.Clear();
            tokenSource?.Cancel();
        }


        private readonly HashSet<Player> _playersInArea = new();
        private readonly HashSet<Player> _DUCKMembers = new();

        private IEnumerator<float> PlayerEscapeCheck(CancellationToken token)
        {
            while (!token.IsCancellationRequested && Round.IsStarted && !Round.IsEnded || !Warhead.IsInProgress ||
                   !Warhead.IsDetonated || IsSCP1356Captured) 
            {
                var currentPlayersInArea = new HashSet<Player>();
                var DUCKMembers = new HashSet<Player>();
                
                float range = Plugin.Singleton.SCP1356Breach ? 8.5f : 3.8f;

                foreach (var player in Player.List.Where(p =>
                             p.IsAlive &&
                             Vector3.Distance(p.Position, SCP1356.Position) < range))
                {
                    DUCKMembers.Add(player);
                    currentPlayersInArea.Add(player);

                    if (player.Role.Type != RoleTypeId.Tutorial && player.Role.Team != Team.SCPs)
                    {
                        player.EnableEffect(effectone, effectDuration);
                        player.EnableEffect(effecttwo, effectDuration);
                        player.EnableEffect(effectthree, effectDuration);
                        player.Hurt(15f, "SCP-1356");
                        if (!_playersInArea.Contains(player))
                        {
                            SCP1356.Position.SpecialPos("1356.ogg", 15, 5);
                        }
                    }
                    else if (player.Role.Type == RoleTypeId.Tutorial)
                    {
                        if (!_DUCKMembers.Contains(player))
                        {
                            if (!Plugin.Singleton.SCP1356Breach)
                            {
                                if (!IsSCP1356GlassOpen)
                                {
                                    player.ShowMeowHint(
                                        "⚠️ <b>SCP-1356 ist noch eingeschlossen!</b> \n" +
                                        "🎯 Schieß auf das <b>dunklere Glas</b>, um das Containment zu öffnen. \n" +
                                        "🦆 Anschließend drücke <b>E</b> auf die Ente, um fortzufahren!");
                                }
                            }
                            else if (Plugin.Singleton.SCP1356Breach || IsSCP1356GlassOpen)
                            {
                                player.ShowMeowHint(
                                    "✅ <b>SCP-1356 ist frei!</b> \n" +
                                    "🦆 Drücke <b>E</b> auf die Ente, um der D.U.C.K einen Respawn zu sichern!");
                            }
                        }
                    }

                }

                _DUCKMembers.Clear();
                _DUCKMembers.UnionWith(DUCKMembers);
                _playersInArea.Clear();
                _playersInArea.UnionWith(currentPlayersInArea);

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
