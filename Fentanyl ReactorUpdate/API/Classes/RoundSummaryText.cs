using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using Fentanyl_ReactorUpdate.API.Extensions;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class RoundSummaryText
    {
        public void SubEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public void UnsubEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void OnRoundEnded(RoundEndedEventArgs obj)
        {
            if (!Plugin.Singleton.Config.RoundSummaryFentanyl)
                return;

            var consumers = CustomItems.FentT1.FentItemConsumers;

            if (!consumers.Any())
            {
                Log.Info("No Fent Items were consumed this round.");
                return;
            }
            
            var topConsumers = consumers
                .OrderByDescending(kvp => kvp.Value)
                .Take(5)
                .Select(kvp => Plugin.Singleton.Translation.RoundSummaryHintPlayers
                    .Replace("{PlayerNickname}", kvp.Key.Nickname)
                    .Replace("{FentanylItems}", kvp.Value.ToString()));
            
            string joinedConsumers = string.Join("\n", topConsumers);
            
            string message;
            if (Plugin.Singleton.Translation.RoundSummaryHint.Contains("{FentanylConsumers}"))
            {
                message = Plugin.Singleton.Translation.RoundSummaryHint
                    .Replace("{FentanylConsumers}", joinedConsumers);
            }
            else
            {
                message = Plugin.Singleton.Translation.RoundSummaryHint;
            }
            
            foreach (var player in Player.List)
            {
                player.ShowHint(message, Plugin.Singleton.Config.RoundSummaryHintDuration); 
            }

            Log.Info(message);
            
            consumers.Clear();
        }
    }
}