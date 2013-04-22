using System;
using System.Collections.Generic;
using System.Linq;

namespace Mike.Spikes.Puzzlers
{
    public class PalindromeQuestion
    {
        public void ProceduralImplementation()
        {
            var max = 0;
            for (var i = 10; i <= 99; i++)
            {
                for (var j = i; j <= 99; j++)
                {
                    var p = i*j;
                    if (p.IsPalindrome2() && p > max) max = p;
                }
            }
            Console.Out.WriteLine("max = {0}", max);
        }

        public void LinqImplementation()
        {
            var max = (
                from a in Enumerable.Range(10, 90) // confusingly (start, count) rather (start, end)
                from b in Enumerable.Range(a, 100-a)
                let p = a*b
                where p.IsPalindrome2()
                select p).Max();

            Console.Out.WriteLine("max = {0}", max);
        }

        public void LinqImplementation2()
        {
            var max = Enumerable.Range(10, 90)
                .SelectMany(i => Enumerable.Range(i, 100-i), (i, j) => i * j)
                .Where(product => product.IsPalindrome2())
                .Max();
            Console.WriteLine(max);
        }
    }

    public static class PalindromeExtensions
    {
        public static bool IsPalindrome(this int value)
        {
            var valueAsString = value.ToString();
            return value.ToString().Equals(valueAsString.Reverse());
        }

        public static bool IsPalindrome2(this int value)
        {
            var digits = value.ToDecimalArray().ToArray();
            var start = 0;
            var end = digits.Length - 1;
            while (start < end)
            {
                if (digits[start] != digits[end]) return false;
                start++;
                end--;
            }
            return true;
        }

        public static IEnumerable<int> ToDecimalArray(this int input)
        {
            var n = input;
            while (n != 0)
            {
                yield return n%10;
                n /= 10;
            }
        }

        public static void Spike()
        {
            foreach (var i in 12345.ToDecimalArray())
            {
                Console.Out.WriteLine(i);
            }

            Console.Out.WriteLine(12321.IsPalindrome2());
            Console.Out.WriteLine(123454321.IsPalindrome2());
            Console.Out.WriteLine(1221.IsPalindrome2());
            Console.Out.WriteLine(123.IsPalindrome2());
            Console.Out.WriteLine(1.IsPalindrome2());
        }
    }
}