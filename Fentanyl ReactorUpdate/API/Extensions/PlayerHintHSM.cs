﻿using Exiled.API.Features;

using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using MEC;
using Utils.Networking;

namespace Fentanyl_ReactorUpdate.API.Extensions
{
    public static class PlayerExtensions
    {
        public static void ShowMeowHint(this Player player, string text)
        {
            PlayerDisplay playerDisplay = PlayerDisplay.Get(player);

            DynamicHint hint = new()
            {
                Text = text,
                TargetY = Plugin.Singleton.Config.GlobalHintY,
                FontSize = Plugin.Singleton.Config.GlobalHintSize,
                SyncSpeed = HintSyncSpeed.Fast,
            };
            playerDisplay.RemoveHint(hint);
            playerDisplay.AddHint(hint);
            Timing.CallDelayed( Plugin.Singleton.Config.GlobalHintDuration,
                () =>
                {
                    playerDisplay.RemoveHint(hint);
                });
        }
        public static void ShowMeowHintMoney(this Player player, string text)
        {
            PlayerDisplay playerDisplay = PlayerDisplay.Get(player);

            DynamicHint hint = new()
            {
                Text = text,
                TargetY = -5, 
                TargetX = 7, 
                FontSize = Plugin.Singleton.Config.GlobalHintSize,
                SyncSpeed = HintSyncSpeed.Fast,
            };
            playerDisplay.RemoveHint(hint);
            playerDisplay.AddHint(hint);
            Timing.CallDelayed( Plugin.Singleton.Config.GlobalHintDuration,
                () =>
                {
                    playerDisplay.RemoveHint(hint);
                });
        }
        public static void ShowMeowHintDur(this Player player, string text, float Dur)
        {
            PlayerDisplay playerDisplay = PlayerDisplay.Get(player);

            DynamicHint hint = new()
            {
                Text = text,
                TargetY = Plugin.Singleton.Config.GlobalHintY,
                FontSize = Plugin.Singleton.Config.GlobalHintSize,
                SyncSpeed = HintSyncSpeed.Fast,
            };
            playerDisplay.RemoveHint(hint);
            playerDisplay.AddHint(hint);
            Timing.CallDelayed(Dur,
                () =>
                {
                    playerDisplay.RemoveHint(hint);
                });
        }
        
        public static void ShowMeowHintExtra(this Player player, string text, string ID)
        {
            PlayerDisplay playerDisplay = PlayerDisplay.Get(player);

            DynamicHint hint = new()
            {
                Id = ID,
                Text = text,
                TargetY = Plugin.Singleton.Config.GlobalHintY,
                FontSize = Plugin.Singleton.Config.GlobalHintSize,
                SyncSpeed = HintSyncSpeed.Fast,
            };
            playerDisplay.AddHint(hint);
            Timing.CallDelayed(Plugin.Singleton.Config.GlobalHintDuration,
                () =>
                {
                    playerDisplay.RemoveHint(hint);
                });
        }
    }
}

// public static void ShowMeowHint(this Player player, string text, string id, float Y, float X, int size)
// {
   // var playerDisplay = PlayerDisplay.Get(player.ReferenceHub);
    // var NameHint = new HintServiceMeow.Core.Models.Hints.Hint()
    // {
       // Id = id,
       // Text = text,
        // Alignment = HintAlignment.Center,
        // YCoordinate = Y,
        // XCoordinate = X,
        // FontSize = size,
    // };
    // player.AddHint(NameHint);
// }
