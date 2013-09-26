using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mike.Spikes.DangersOfThreading
{
    public class RaceCondition
    {
        private const int numberOfIterations = 5000;
        private const int numberOfTasks = 10;

        // shared state accessed by many threads
        private int sharedState = 0;

        public void Start()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                tasks.Add(Task.Factory.StartNew(Work, TaskCreationOptions.LongRunning));
                //tasks.Add(Task.Factory.StartNew(WorkWithLock, TaskCreationOptions.LongRunning));
            }

            Task.WaitAll(tasks.ToArray());

            Console.Out.WriteLine("Expected shared state = {0}", numberOfIterations * numberOfTasks);
            Console.Out.WriteLine("sharedState =           {0}", sharedState);
        }

        public void Work()
        {
            for (var i = 0; i < numberOfIterations; i++)
            {
                var myLocalCopyOfState = sharedState;

                DoSomeOtherWork();

                myLocalCopyOfState++;

                sharedState = myLocalCopyOfState;
            }
        }

        private readonly object @lock = new object();

        public void WorkWithLock()
        {
            // serialize access to critical section with a lock.
            lock (@lock)
            {
                Work();        
            }
        }

        public void DoSomeOtherWork()
        {
            int count = 0;
            for (int j = 0; j < 1000; j++)
            {
                count++;
            }
            
        }
    }
}