using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System;

namespace Mike.Spikes.ObservablePlay
{
    public class RxProducerConsumer
    {
        public void JustUsingSubject()
        {
            Log.WriteLine("Starting ----");
            var subject = new Subject<string>();

            var scheduler = Scheduler.Immediate;

            subject.ObserveOn(scheduler).Subscribe(DoWork);
            subject.ObserveOn(scheduler).Subscribe(DoWork);

            for (int i = 0; i < 5; i++)
            {
                new System.Threading.Thread(state =>
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            System.Threading.Thread.Sleep(0);
                            subject.OnNext(string.Format("{0} -> {1} ", state, j));
                        }
                    }){ Name = "Dispatcher " + i }.Start(string.Format("Thread {0}", i));
            }

            Log.WriteLine("Ending ----");

            System.Threading.Thread.Sleep(1000);
        }

        public void DoWork(string message)
        {
            Log.Write(message);
            System.Threading.Thread.Sleep(0);
            Log.WriteLine("--END {0}", message);
        }
    }
}