using System;
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
                    if (p.IsPalindrome() && p > max) max = p;
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
                where p.IsPalindrome()
                select p).Max();

            Console.Out.WriteLine("max = {0}", max);
        }

        public void LinqImplementation2()
        {
            var max = Enumerable.Range(10, 90)
                .SelectMany(i => Enumerable.Range(i, 100-i), (i, j) => i * j)
                .Where(product => product.IsPalindrome())
                .Max();
            Console.WriteLine(max);
        }
    }

    public static class PalindromeExtensions
    {
        public static bool IsPalindrome(this int value)
        {
            var valueAsString = value.ToString();
            return valueAsString == new string(valueAsString.Reverse().ToArray());
        }
    }
}