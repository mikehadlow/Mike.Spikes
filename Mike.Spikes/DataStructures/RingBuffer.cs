using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mike.Spikes.DataStructures
{


    public class RingBuffer
    {
        public static IEnumerable<int> Integers()
        {
            var counter = 0;
            while (true)
            {
                yield return counter;
                counter++;
            }
        }

        public static void DemoIntegers()
        {
            var upTo10 = Integers().Where(x => x % 2 == 0).Take(10);

            foreach (var i in upTo10)
            {
                Console.Out.WriteLine("i = {0}", i);
            }
        }

        public void Test()
        {
            var ring = new RingBuffer<string>();

            ring.Add("one");
            ring.Add("two");
            ring.Add("three");
            ring.Add("four");
            ring.Add("five");

            foreach (var item in ring.Take(30))
            {
                Console.Out.WriteLine("{0}", item);
            }
        } 
    }

    public class RingBuffer<T> : IEnumerable<T>
    {
        private RingNode<T> current;

        public void Add(T item)
        {
            if (current == null)
            {
                current = new RingNode<T>(item);
                current.Next = current;
            }
            else
            {
                var ringNode = new RingNode<T>(item) {Next = current.Next};
                current.Next = ringNode;
                current = ringNode;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new RingBufferEnumerator<T>(current);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class RingNode<T>
    {
        public T Value { get; private set; }
        public RingNode<T> Next { get; set; } 

        public RingNode(T value)
        {
            Value = value;
        }
    }

    public class RingBufferEnumerator<T> : IEnumerator<T>
    {
        private RingNode<T> currentNode; 

        public RingBufferEnumerator(RingNode<T> currentNode)
        {
            this.currentNode = currentNode;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (currentNode == null) return false;
            currentNode = currentNode.Next;
            return true;
        }

        public void Reset()
        {
        }

        public T Current 
        {
            get { return currentNode.Value; } 
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}