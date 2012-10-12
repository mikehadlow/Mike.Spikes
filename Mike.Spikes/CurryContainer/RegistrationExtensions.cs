using System;

namespace Mike.Spikes.CurryContainer
{
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