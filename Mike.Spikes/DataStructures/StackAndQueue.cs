using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.DataStructures
{
    public class StackAndQueue
    {
        /// <summary>
        /// Use stack anywhere you need last-in first-out 
        /// </summary>
        public void StackDemo()
        {
            var stack = new Stack<string>();

            stack.Push("one");
            stack.Push("two");
            stack.Push("three");

            Console.Out.WriteLine(stack.Pop());

            stack.Push("four");

            Console.Out.WriteLine(stack.Pop());
            Console.Out.WriteLine(stack.Pop());
            Console.Out.WriteLine(stack.Pop());
        }

        /// <summary>
        /// Use queue for first-in first-out
        /// </summary>
        public void QueueDemo()
        {
            var queue = new Queue<string>();

            queue.Enqueue("one");
            queue.Enqueue("two");
            queue.Enqueue("three");

            Console.Out.WriteLine(queue.Dequeue());

            queue.Enqueue("four");

            Console.Out.WriteLine(queue.Dequeue());
            Console.Out.WriteLine(queue.Dequeue());
            Console.Out.WriteLine(queue.Dequeue());
        }

        /// <summary>
        /// Blocking collection acts as a threadsafe blocking queue
        /// </summary>
        public void BlockingCollection()
        {
            var queue = new BlockingCollection<string>();

            Task.Run(() =>
                {
                    while (true)
                    {
                        Console.Out.WriteLine(queue.Take());
                    }
                });

            for (int i = 0; i < 5; i++)
            {
                queue.Add(string.Format("item {0}", i));
                Thread.Sleep(1000);
            }

            queue.Dispose();
        }
    }
}