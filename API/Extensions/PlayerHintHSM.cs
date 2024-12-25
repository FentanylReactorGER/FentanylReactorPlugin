using Exiled.API.Features;
using Utils.Networking;

namespace Fentanyl_ReactorUpdate.API.Extensions
{
    public static class PlayerExtensions
    {
        public static void ShowMeowHint(this Player player, string text)
        {
            player.ShowHint(text, Plugin.Singleton.Config.GlobalHintDuration);
        }
    }
}
