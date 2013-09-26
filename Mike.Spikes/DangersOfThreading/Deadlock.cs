using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.DangersOfThreading
{
    public class Deadlock
    {
        public void Start()
        {
            Console.Out.WriteLine("Starting");

            var task1 = Task.Factory.StartNew(FirstWork, TaskCreationOptions.LongRunning);
            var task2 = Task.Factory.StartNew(SecondWork, TaskCreationOptions.LongRunning);

            Task.WaitAll(task1, task2);

            Console.Out.WriteLine("Ended");
        }

        private readonly object taskALock = new object();
        private readonly object taskBLock = new object();

        public void FirstWork()
        {
            lock (taskALock)
            {
                Console.Out.WriteLine("First worker inside taskALock");

                Thread.Sleep(10);

                Console.Out.WriteLine("First worker waiting for taskBLock");
                lock (taskBLock)
                {
                    Console.Out.WriteLine("First worker inside taskBLock");    
                }
            }
        }

        public void SecondWork()
        {
            lock (taskBLock)
            {
                Console.Out.WriteLine("Second worker inside taskBLock");

                Thread.Sleep(10);

                Console.Out.WriteLine("Second worker waiting for taskALock");
                lock (taskALock)
                {
                    Console.Out.WriteLine("Second worker inside taskALock");
                }
            }
        }
    }
}