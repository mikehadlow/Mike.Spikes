using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mike.Spikes
{
    public class PalindromeQuestion
    {
        public void ProceduralImplementation()
        {
            Func<IEnumerable<int>> range = () => Enumerable.Range(10, 90);

            var max = range()
                .SelectMany(i => range(), (i, j) => i*j)
                .Where(product => product.IsPalindrome())
                .Concat(new[] {0}).Max();
            Console.WriteLine(max);
        }

        public void LinqImplementation()
        {

        }
    }

    public static class PalindromeExtensions
    {
        public static bool IsPalindrome(this int value)
        {
            var valueAsString = value.ToString(CultureInfo.InvariantCulture);
            return valueAsString == new string(valueAsString.Reverse().ToArray());
        }
    }
}