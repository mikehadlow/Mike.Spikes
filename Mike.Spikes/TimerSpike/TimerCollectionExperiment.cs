using System;
using System.Threading;

namespace Mike.Spikes.TimerSpike
{
    public class TimerCollectionExperiment
    {
        public void Test()
        {
            StartTimer();

            Thread.Sleep(1000);
            GC.Collect();
            Console.Out.WriteLine("Collection");

            Thread.Sleep(2000);
        }

        public void StartTimer()
        {
            var timer = new Timer(Callback);
            timer.Change(2000, Timeout.Infinite);
            Console.Out.WriteLine("Created Timer");
        }

        private void Callback(object state)
        {
            var timer = state as Timer;
            if (timer != null)
            {
                timer.Dispose();
            }
            Console.Out.WriteLine("Timer Fired");
        }
    }
}