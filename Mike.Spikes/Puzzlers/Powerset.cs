using System;
using System.Linq;
using System.Collections.Generic;

namespace Mike.Spikes.Puzzlers
{
    public static class PowersetExtensions
    {
        public static IList<IList<T>> Powerset<T>(this IList<T> input)
        {
            return new List<IList<T>>();
        }

        public static IEnumerable<T> Cons<T>(this T head, IEnumerable<T> tail)
        {
            yield return head;
            foreach (var item in tail)
            {
                yield return item;
            }
        } 
    }

    public class PowersetTests
    {
        public void Test()
        {
            var result = new List<int> {1, 2, 3}.Powerset();

            foreach (var list in result)
            {
                Console.Write("[");
                Console.Write(string.Join(",", list));
                Console.WriteLine("]");
            }
        }

        public void Spike()
        {
            var input = new List<int> {1, 2, 3};

            var x = from item in input
                    from b in new List<bool> { true, false }
                    select new { item, b };

            foreach (var item in x)
            {
                Console.WriteLine("{0} {1}", item.item, item.b);
            }
        }

        public void TruthTable()
        {
            var result = Truth(new List<int>{0,1,2}, 3);

            foreach (var list in result)
            {
                Console.Write("[");
                Console.Write(string.Join(",", list));
                Console.WriteLine("]");
            }
        }

        public static IEnumerable<IEnumerable<T>> Truth<T>(IList<T> elements, int depth)
        {
            if (depth == 0)
            {
                yield return Enumerable.Empty<T>();
                yield break;
            }
            if (depth == 1)
            {
                foreach (var element in elements)
                {
                    yield return element.Cons(Enumerable.Empty<T>());
                }
                yield break;
            }

            var newDepth = --depth;
            foreach (var element in elements)
            {
                foreach (var subList in Truth(elements, newDepth))
                {
                    yield return element.Cons(subList);
                }
            }
        } 
    }
}