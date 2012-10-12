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

    public static class RegistrationExtensions
    {
        public static IRegistration Register<A,R>(this IRegistration registration, Func<A, R> source)
        {
            var targetType = typeof (Func<A, R>);
            var curried = Functional.Curry(source);

            registration.Add(targetType, () => curried);

            return registration;
        }

        public static IRegistration Register<A,B,R>(this IRegistration registration, Func<A, B, R> source)
        {
            var targetType = typeof (Func<B, R>);
            var curried = Functional.Curry(source);

            registration.Add(targetType, () => curried(
                registration.Get<A>()
                ));

            return registration;
        }

        public static IRegistration Register<A, B, C, R>(this IRegistration registration, Func<A, B, C, R> source)
        {
            var targetType = typeof(Func<C, R>);
            var curried = Functional.Curry(source);

            registration.Add(targetType, () => curried(
                registration.Get<A>()
                )
                (
                registration.Get<B>()
                ));

            return registration;
        }
    }
}