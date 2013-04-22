using System;
using System.Collections.Generic;
using System.Linq;

namespace Mike.Spikes.Puzzlers
{
    public class FizzBuzz
    {
        public void Linq()
        {
            var result =
                from n in HandyFunctions.NaturnalNumbers().Take(15)
                select
                    n % 15 == 0 ? "FizzBuzz" :
                    n % 3 == 0 ? "Fizz" :
                    n % 5 == 0 ? "Buzz" :
                    n.ToString();

            Console.Out.WriteLine(string.Join(", ", result));
        }     
    }

    public static class HandyFunctions
    {
        // I don't like enumerable range
        public static IEnumerable<int> NaturnalNumbers()
        {
            var n = 0;
            while (true)
            {
                yield return ++n;
            }
        }
    }
}