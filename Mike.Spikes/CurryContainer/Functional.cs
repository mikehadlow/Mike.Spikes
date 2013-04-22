using System;

namespace Mike.Spikes.CurryContainer
{
    public static class Functional
    {
        public static Func<A, R> Curry<A, R>(Func<A, R> input)
        {
            return input;
        }

        public static Func<A, Func<B, R>> Curry<A, B, R>(Func<A, B, R> input)
        {
            return a => b => input(a, b);
        }

        public static Func<A, Func<B, Func<C,R>>> Curry<A, B, C, R>(Func<A, B, C, R> input)
        {
            return a => b => c => input(a, b, c);
        }
    }
}