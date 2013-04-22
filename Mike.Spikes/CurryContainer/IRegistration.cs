using System;
using System.Collections.Generic;

namespace Mike.Spikes.CurryContainer
{
    public interface IRegistration
    {
        void Add(Type target, Func<object> constructor);
        T Get<T>();
    }

    public class Container : IRegistration
    {
        private readonly Dictionary<Type, Func<object>> registrations = new Dictionary<Type, Func<object>>();

        public void Add(Type target,  Func<object> constructor)
        {
            registrations.Add(target, constructor);
        }

        public T Get<T>()
        {
            return (T)registrations[typeof (T)]();
        }
    }
}