using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.Threading
{
    public class ManyWaysToStartAThread
    {
        public void WithThread()
        {
            // explicit thread creation doesn't run on the threadpool.

            var thread = new Thread(() =>
                {
                    Console.WriteLine("Hello from {0} {1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
                })
                {
                    Name = "my thread"
                };

            thread.Start();
            Console.Out.WriteLine("Hello from the calling thread {0}", Thread.CurrentThread.ManagedThreadId);
            thread.Join();
        }

        public void WithDelegate()
        {
            // delegates expose the APM. Thread runs on the threadpool.

            Action myDelegate = () =>
                {
                    Console.WriteLine("Hello from the delegate {0}", Thread.CurrentThread.ManagedThreadId);
                };

            var asyncResult = myDelegate.BeginInvoke(state =>
                {
                    Console.Out.WriteLine("Hello from the async callback {0}", Thread.CurrentThread.ManagedThreadId);
                }, null);

            Console.Out.WriteLine("Hello from the calling thread {0}", Thread.CurrentThread.ManagedThreadId);
            myDelegate.EndInvoke(asyncResult);
        }

        public void WithThreadpool()
        {
            var autoResetEvent = new AutoResetEvent(false);

            ThreadPool.QueueUserWorkItem(state =>
                {
                    Console.WriteLine("Hello from the thread pool {0}", Thread.CurrentThread.ManagedThreadId);
                    autoResetEvent.Set();
                });

            Console.Out.WriteLine("Hello from the calling thread {0}", Thread.CurrentThread.ManagedThreadId);
            autoResetEvent.WaitOne();
        }

        public void WithTpl()
        {
            var task = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Hello from the task {0}", Thread.CurrentThread.ManagedThreadId);
                }, TaskCreationOptions.LongRunning);

            Console.Out.WriteLine("Hello from the calling thread {0}", Thread.CurrentThread.ManagedThreadId);
            task.Wait();
        }

        public void WithParallel()
        {
            var autoResetEvent = new AutoResetEvent(false);
            Parallel.Invoke(() =>
                {
                    Console.WriteLine("Hello from the thread {0}", Thread.CurrentThread.ManagedThreadId);
                    autoResetEvent.Set();
                });

            Console.Out.WriteLine("Hello from the calling thread {0}", Thread.CurrentThread.ManagedThreadId);
            autoResetEvent.WaitOne();
        }
    }
}