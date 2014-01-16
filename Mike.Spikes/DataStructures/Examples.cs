using System;
using System.Collections.Generic;
using System.Linq;

namespace Mike.Spikes.DataStructures
{
    /*
     * 
     * Big O Notation
     * 
     * Generally n is the number of items O(f(n)) where f is some function over n
     * 
     * O(1)     "Constant"      The operation always takes the same amount of time no matter the number of items.
     * O(log n) "Logarithmic"   Fiding an item in a sorted set with a binary search.
     *                          This is the theoretical limit for searching a dataset.
     * O(n)     "Linear"        The time the operation takes increases linearly with the number of times.
     * 
     * O(n^2)   "Quadratic"     E.g. bubble sort naive implementation
     * O(n!)    "Factoral"      Enumerating all partitions of a set
     * 
     * 
     */

    public class Examples
    {
        const int size = 5;

        // Avoid arrays unless you know you really need them (like byte[])
        // explain layout in memory
        public void Array()
        {
            // lookup:  O(1)
            // modify:  O(1)

            var array = new Thing[size];
            Console.Out.WriteLine("Initialised");
            array.Print();

            Console.Out.WriteLine("Populated");
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Thing(string.Format("Item {0}", i));
            }
            array.Print();

            var copy = new Thing[size];
            array.CopyTo(copy, 0);

            Console.Out.WriteLine("Item 3 updated");
            array[3].Name = "Updated";
            //array[3] = new Thing("Updated");
            array.Print();

            Console.Out.WriteLine("Copy");
            copy.Print();
        }

        // System.Collections.Generic

        // Your 'goto' collection datastructure. "If in doubt, List throughout."
        public void List()
        {
            // lookup:  Index O(1)
            //          Value O(n)
            // modify         O(n)

            Console.Out.WriteLine("New");
            var list = new List<Thing>(size);
            list.Print();

            Console.Out.WriteLine("Populated");
            for (int i = 0; i < size; i++)
            {
                list.Add(new Thing(string.Format("Item {0}", i)));
            }
            // list.AddRange(Enumerable.Range(0, size).Select(i => new Thing(string.Format("Item {0}", i))));
            list.Print();

            Console.Out.WriteLine("Item 3 updated");
            list[3] = new Thing("Updated");
            list.Print();

            Console.Out.WriteLine("Print updated");
            var updated = from item in list
                          where item.Name == "Updated"
                          select item;

            updated.Print();
        }

        // Very efficient lookups. 
        // Can have multiple dictionaries on a single object collection.
        public void Dictionary()
        {
            // lookup:  O(1)
            // modify:  O(1)

            Console.Out.WriteLine("New");
            var dictionary = new Dictionary<int, Thing>();
            dictionary.Print();

            Console.Out.WriteLine("Populated");
            for (int i = 0; i < size; i++)
            {
                dictionary.Add(i, new Thing(string.Format("Item {0}", i)));
            }
            dictionary.Print();

//            Console.Out.WriteLine("Alternative dictionary creation");
//            var dictionary2 = Enumerable.Range(0, size).ToDictionary(
//                x => x, 
//                x => new Thing(string.Format("Item {0}", x)));
//            dictionary2.Print();

            Console.Out.WriteLine("Keys");
            dictionary.Keys.Print();

            Console.Out.WriteLine("Values");
            dictionary.Values.Print();
        }

        // Very efficient for set based operations. 
        // A bit like having just the keys of a Dictionary
        public void HashSet()
        {
            // lookup:  O(1)
            // modify:  O(1)

            Console.Out.WriteLine("New");
            var set = new HashSet<Thing>(new ThingEqualityComparer());
            set.Print();

            Console.Out.WriteLine("Populated");
            for (int i = 0; i < size; i++)
            {
                set.Add(new Thing(string.Format("Item {0}", i)));
            }
            set.Print();

            if(!set.Add(new Thing("New Thing")))
            {
                Console.Out.WriteLine("Couldn't add New Thing");
            }

            // this is a great way of checking if you've seen something before
            if(!set.Add(new Thing("Item 3")))
            {
                Console.Out.WriteLine("Couldn't add Item 3");
            }
        }

        // very efficient dynamic creation of sorted items
        public void SortedSet()
        {
            // lookup:  O(log n)
            // modify:  O(log n)

            Console.Out.WriteLine("New");
            var set = new SortedSet<Thing>(new ThingComparer());
            set.Print();

            Console.Out.WriteLine("Populated");
            set.Add(new Thing("Thing 10"));
            set.Add(new Thing("Thing 0"));
            set.Add(new Thing("Thing 4"));
            set.Add(new Thing("Thing 3"));
            set.Add(new Thing("Thing 2"));
            set.Print();
        }

        // Very efficient for insertion/removal at any point
        public void LinkedList()
        {
            // lookup:  O(n)
            // modify:  O(1) 

            Console.Out.WriteLine("New");
            var linkedList = new LinkedList<Thing>();
            linkedList.Print();

            Console.Out.WriteLine("Populated");
            for (var i = 0; i < size; i++)
            {
                linkedList.AddLast(new Thing(string.Format("Item {0}", i)));
            }
            linkedList.Print();

            Console.Out.WriteLine("Append new item to front of list");
            linkedList.AddFirst(new Thing("New"));
            linkedList.Print();

            Console.Out.WriteLine("Remove the head of the list");
            linkedList.RemoveFirst();
            linkedList.Print();
        }
    }

    public class Thing
    {
        public string Name { get; set; }

        public Thing(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("Thing: {0}", Name);
        }
    }

    public class ThingEqualityComparer : IEqualityComparer<Thing>
    {
        public bool Equals(Thing x, Thing y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Thing obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class ThingComparer : IComparer<Thing>
    {
        public int Compare(Thing x, Thing y)
        {
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }

    public static class Utils
    {
        public static void Print<T>(this IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Console.Out.WriteLine(item == null ? "Null" : item.ToString());
            }
            Console.Out.WriteLine("");
        }

        public static void Print<K, V>(this IDictionary<K, V> items)
        {
            foreach (var item in items)
            {
                Console.Out.WriteLine("{0} = {1}",
                    item.Key,
                    item.Value == null ? "Null" : item.Value.ToString());
            }
            Console.Out.WriteLine("");
        }
    }
}