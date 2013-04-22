using System;
using System.Threading;

namespace Mike.Spikes.ConsoleShutdown
{
    class Program
    {
        private static bool cancel = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Application has started. Ctrl-C to end");

            // do some cool stuff here
            var myThread = new Thread(Worker);
            myThread.Start();

            var autoResetEvent = new AutoResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    // cancel the cancellation to allow the program to shutdown cleanly
                    eventArgs.Cancel = true;
                    autoResetEvent.Set();
                };

            // main blocks here waiting for ctrl-C
            autoResetEvent.WaitOne();
            cancel = true;
            Console.WriteLine("Now shutting down");
        }

        private static void Worker()
        {
            while (!cancel)
            {
                Console.WriteLine("Worker is working");
                Thread.Sleep(1000);
            }
            Console.WriteLine("Worker thread ending");
        }
    }
}
