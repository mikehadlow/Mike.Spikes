using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mike.Spikes.Linq
{
    public class LinqToSql
    {
        // IEnumerable<T> and IEnumerator<T>
        public void EnumerableDemo()
        {
            var thing = new Thing();

            var result = thing
                .GetFirst(10)
                .Filter(x => x%2 == 0)
                .Map(x => string.Format("{0} x 10 = {1}", x, x * 10));

            foreach (var item in result)
            {
                Console.Out.WriteLine(item);
            }
        }

        // yield return
        public void YieldReturnDemo()
        {
            foreach (var number in Numbers().GetFirst(10))
            {
                Console.Out.WriteLine(number);
            }   
        }

        // extension methods on IEnumerable<T> 
        public void ExtensionMethodDemo()
        {
        }

        // What is the maximum palindromic number that is a product of two two digit numbers?
        public void PalindromeQuestion()
        {
            var list1 = Enumerable.Range(10, 90);
            var list2 = Enumerable.Range(10, 90);

            var results = from item1 in list1
                          from item2 in list2
                          let x = item1 * item2
                          where (x).IsPalindrome()
                          select x;

            Console.Out.WriteLine(results.Max());

            var extResult = list1.SelectMany(x => list2.Select(y => y*x))
                                 .Where(x => x.IsPalindrome()).Max();

            Console.Out.WriteLine(extResult);

            Console.Out.WriteLine(202.IsPalindrome());
            Console.Out.WriteLine(1234321.IsPalindrome());
            Console.Out.WriteLine(10002.IsPalindrome());
        }

        public static IEnumerable<int> Numbers()
        {
            var current = 0;
            while (true)
            {
                yield return current++;
            }
        } 
    }

    public static class ThingExtensions
    {
        public static IEnumerable<T> GetFirst<T>(this IEnumerable<T> input, int count)
        {
            var currentCount = 0;
            foreach (var item in input)
            {
                yield return item;
                currentCount++;
                if (currentCount > count)
                {
                    break;
                }
            }
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> input, Func<T, bool> func)
        {
            foreach (var item in input)
            {
                if (func(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> input, Func<A, B> func)
        {
            foreach (var item in input)
            {
                yield return func(item);
            }
        }

    }

    public class Thing : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            return new ThingEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ThingEnumerator : IEnumerator<int>
    {
        public void Dispose()
        {}

        public bool MoveNext()
        {
            Current++;
            return true;
        }

        public void Reset()
        {
            Current = 0;
        }

        public int Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}