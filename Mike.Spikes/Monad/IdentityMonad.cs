using System;

namespace Mike.Spikes.Monad
{
    public class IdentityMonad
    {
        public void FunctionComposition()
        {
            Func<int, int> add5 = x => x+5;
            Func<int, int> mult2 = x => x*2;
            Func<int, int> add5Mult2 = x => mult2(add5(x));

            Console.Out.WriteLine("add5mult2(4) = {0}", add5Mult2(4));

            Func<int, Identity<int>> iadd5 = x => (x + 5).ToIdentity();
            Func<int, Identity<int>> imult2 = x => (x*2).ToIdentity();
            Func<int, Identity<int>> iadd5Mult2 = x => iadd5(x).Bind(imult2);

            Console.Out.WriteLine("iadd5Mult2(4).Value = {0}", iadd5Mult2(4).Value);

            var result = 
                5.ToIdentity().Bind(x =>
                "Hello World".ToIdentity().Bind(y =>
                DateTime.Now.ToIdentity().Bind(z => 
                    string.Format("{0} {1} {2}", x, y, z).ToIdentity())));

            Console.Out.WriteLine("result.Value = {0}", result.Value);

            var linqResult =
                from a in 5.ToIdentity()
                from b in "Hello World".ToIdentity()
                from c in DateTime.Now.ToIdentity()
                select string.Format("{0} {1} {2}", a, b, c);

            Console.Out.WriteLine("linqResult.Value = {0}", linqResult.Value);
        }
    }


    public class Identity<T>
    {
        public T Value { get; private set; }

        public Identity(T value)
        {
            Value = value;
        }
    }

    public static class IdentityExtensions
    {
        public static Identity<T> ToIdentity<T>(this T value)
        {
            return new Identity<T>(value);
        } 

        public static Identity<B> Bind<A, B>(this Identity<A> a, Func<A, Identity<B>> func)
        {
            return func(a.Value);
        }

        public static Identity<C> SelectMany<A, B, C>(this Identity<A> a, Func<A, Identity<B>> func,
            Func<A, B, C> select)
        {
            return a.Bind(avalue => func(avalue).Bind<B,C>(bvalue => select(avalue, bvalue).ToIdentity()));
        } 
    }
}