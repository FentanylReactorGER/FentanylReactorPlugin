using Exiled.API.Features;
using Utils.Networking;

namespace Fentanyl_ReactorUpdate.API.Extensions
{
    public static class PlayerExtensions
    {
        public static void ShowMeowHint(this Player player, string text) // Dear EXILED People, this is in work
        {
            player.ShowHint(text, Plugin.Singleton.Config.GlobalHintDuration);
        }
    }
}
