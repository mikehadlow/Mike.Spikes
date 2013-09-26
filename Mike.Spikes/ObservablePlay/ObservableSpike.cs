using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Mike.Spikes.ObservablePlay
{
    public class ObservableSpike
    {
        public void UseMyObservable()
        {
            var myObservable = new MyObservable();

            var firstUnsubscribe = myObservable.Subscribe(new Observer("First"));

            var secondUnsubscribe = myObservable
                //.ObserveOn(NewThreadScheduler.Default)
                .Subscribe(new Observer("Second"));

            myObservable.Subscribe(message => Log.WriteLine("[Third] {0}", message));

            myObservable.Send("Hello");
            myObservable.Send(".. and again");

            //myObservable.Error("Error message");

            firstUnsubscribe.Dispose();
            secondUnsubscribe.Dispose();
        }

        public void TimerObservable()
        {
            var source = Observable
                .Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                .Timestamp();

            var count = 0;
            var autoResetEvent = new AutoResetEvent(false);

            using (source.Subscribe(x =>
                {
                    Log.WriteLine("{0}: {1}", x.Value, x.Timestamp);
                    count++;
                    if (count == 5)
                    {
                        autoResetEvent.Set();
                    }
                }))
            {
                autoResetEvent.WaitOne(TimeSpan.FromSeconds(10));
            }
        }

        public void EnumerableToObservable()
        {
            var enumerable = new List<int> {1, 2, 3, 4};
            var source = enumerable.ToObservable();
            source.Subscribe(x => Log.WriteLine("A {0}", x));
            source.Subscribe(x => Log.WriteLine("B {0}", x));
            source.Subscribe(x => Log.WriteLine("C {0}", x));
        }

        public void ObservableInterval()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1));
            var sub1 = source.Subscribe(x => Log.WriteLine("A {0}", x));
            Thread.Sleep(TimeSpan.FromSeconds(5));

            var sub2 = source.Subscribe(x => Log.WriteLine("\tB {0}", x));
            Thread.Sleep(TimeSpan.FromSeconds(5));

            sub1.Dispose();
            sub2.Dispose();
        }

        public void ObservablePublish()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1));
            var hot = source.Publish();
            var hotCancelation = hot.Connect();

            var sub1 = hot.Subscribe(x => Log.WriteLine("A {0}", x));
            Thread.Sleep(TimeSpan.FromSeconds(5));

            var sub2 = hot.Subscribe(x => Log.WriteLine("\tB {0}", x));
            Thread.Sleep(TimeSpan.FromSeconds(5));

            hotCancelation.Dispose();
        }

        public void SubjectPlay()
        {
            var subject = new Subject<string>();
            
            subject.ObserveOn(NewThreadScheduler.Default).Subscribe(x => Log.WriteLine("A {0}", x));
            subject.Subscribe(x => Log.WriteLine("B {0}", x));

            subject.OnNext("First");
            subject.OnNext("Second");
            subject.OnNext("Third");
        }

        public void SubscribeOnPlay()
        {
            Log.WriteLine("Starting");
            
            var source = Observable.Create<int>(o =>
                {
                    Log.WriteLine("Invoked");
                    o.OnNext(1);
                    o.OnNext(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Log.WriteLine("Finished");
                    return Disposable.Empty;
                }).ObserveOn(NewThreadScheduler.Default);

            Log.WriteLine("Observable created");

            source
                .Subscribe(
                i => { Log.WriteLine("A Received {0}" , i); Thread.Sleep(10); }, 
                () => Log.WriteLine("A Completed"));

            source
                .Subscribe(
                i => { Log.WriteLine("B Received {0}", i); Thread.Sleep(10); }, 
                () => Log.WriteLine("B Completed"));

            Log.WriteLine("Subscribed");

            Thread.Sleep(1000);
        }

        public void Schedulers()
        {
            var are = new AutoResetEvent(false);

            //var scheduler = Scheduler.Immediate;
            var scheduler = NewThreadScheduler.Default;
            //var scheduler = TaskPoolScheduler.Default;

            var delay = TimeSpan.FromSeconds(1);

            scheduler.Schedule(delay, () =>
                {
                    Log.WriteLine("Hello");
                    are.Set();
                });

            Log.WriteLine("Scheduled");
            are.WaitOne(TimeSpan.FromSeconds(10));
        }

        public void RecursiveScheduling()
        {
            var delay = TimeSpan.FromMilliseconds(10);
            
            Action<Action<TimeSpan>> work = self =>
                {
                    Log.WriteLine("Running");
                    //Thread.Sleep(10);
                    self(delay);
                };

            var scheduler = new EventLoopScheduler();
            
            var token = scheduler.Schedule(delay, work);

            Log.WriteLine("Scheduled");
            Thread.Sleep(100);
            token.Dispose();
            Log.WriteLine("Cancelled");
        }
    }

    public class MyObservable : IObservable<string>
    {
        private readonly IList<IObserver<string>> observers = new List<IObserver<string>>(); 

        public IDisposable Subscribe(IObserver<string> observer)
        {
            observers.Add(observer);
            return new Unsubscriber(observer, this);
        }

        public void Apply(Action<IObserver<string>> actionToApply)
        {
            foreach (var observer in observers)
            {
                actionToApply(observer);
            }
        }

        public void Cancel()
        {
            Apply(o => o.OnCompleted());
        }

        public void Send(string message)
        {
            Log.WriteLine("Starting Send of '{0}'", message);
            Apply(o => o.OnNext(message));
            Log.WriteLine("Ending   Send of '{0}'", message);
        }

        public void Error(string errorMessage)
        {
            Apply(o => o.OnError(new Exception(errorMessage)));
        }

        public class Unsubscriber : IDisposable
        {
            private readonly IObserver<string> observer;
            private readonly MyObservable myObservable;

            public Unsubscriber(IObserver<string> observer, MyObservable myObservable)
            {
                this.observer = observer;
                this.myObservable = myObservable;
            }

            public void Dispose()
            {
                myObservable.observers.Remove(observer);
                observer.OnCompleted();
            }
        }
    }

    public class Observer : IObserver<string>
    {
        private readonly string name;

        public Observer(string name)
        {
            this.name = name;
        }

        public void OnNext(string value)
        {
            Log.WriteLine("[{0}] Got message '{1}'", name, value);
            Thread.Sleep(100);
            Log.WriteLine("[{0}] Completed message '{1}'", name, value);
        }

        public void OnError(Exception error)
        {
            Log.WriteLine("[{0}] Got error '{1}'", name, error.Message);
        }

        public void OnCompleted()
        {
            Log.WriteLine("[{0}] Completed", name);
        }
    }

    public static class Log
    {
        public static void WriteLine(string format, params object[] args)
        {
            var message = String.Format(format, args);
            Console.Out.WriteLine("[Thread {0}:{1}] {2}",
                                  Thread.CurrentThread.Name,
                                  Thread.CurrentThread.ManagedThreadId,
                                  message);
        }

        public static void Write(string format, params object[] args)
        {
            var message = String.Format(format, args);
            Console.Out.Write("[Thread {0}:{1}] {2}",
                                  Thread.CurrentThread.Name,
                                  Thread.CurrentThread.ManagedThreadId,
                                  message);
        }
    }
}