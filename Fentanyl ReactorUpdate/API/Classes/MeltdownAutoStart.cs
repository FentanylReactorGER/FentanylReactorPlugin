using Exiled.API.Features;
using Fentanyl_ReactorUpdate;
using MEC;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class MeltdownAutoStart
    {
        private Reactor _reactor;
        private CoroutineHandle _meltdownStartHandle;

        public MeltdownAutoStart()
        {
            SubEvents();
            _reactor = Plugin.Singleton.Reactor;  
        }

        public void Destroy()
        {
            UnSubEvents();
        }
        
        public void SubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public void UnSubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void OnRoundStarted()
        {
            float StartMeltdownIn = UnityEngine.Random.Range(Plugin.Singleton.Config.MeltdownZeitMinStartRunde, Plugin.Singleton.Config.MeltdownZeitMaxStartRunde);
            float RandomDelay = Plugin.Singleton.Reactor.RandomDelay;
            Log.Info($"Round Started and Reactor meltdown in {StartMeltdownIn} seconds.");
            _meltdownStartHandle = Timing.CallDelayed(StartMeltdownIn, () =>
            {
                if (!_reactor.IsMeltdownTriggered) 
                {
                    _reactor.SetMeltdownTriggered(true); 
                    Log.Info($"Meltdown triggered and explodes in {RandomDelay}.");
                    _reactor.Meltdown(true);  
                }
                else
                {
                    Log.Warn("Meltdown attempt blocked as it has already been triggered.");
                }
            });
        }

        private void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs ev)
        {
            if (_meltdownStartHandle.IsRunning)
            {
                Timing.KillCoroutines(_meltdownStartHandle);
                Log.Info("Meltdown delayed call canceled due to round ending.");
            }
        }
    }
}