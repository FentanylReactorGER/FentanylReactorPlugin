using Exiled.API.Features;
using HintServiceMeow.Core;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using Utils.Networking;

namespace Fentanyl_ReactorUpdate.API.Extensions
{
    public static class PlayerExtensions
    {
        public static void ShowMeowHint(this Player player, string text)
        {
            player.ShowHint(text);
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
