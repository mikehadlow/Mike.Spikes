using System;
using System.Collections.Generic;

namespace Mike.Spikes.Windsor101
{
    public class Bus : IBus, IDisposable
    {
        private readonly IPaymentServiceConfiguration configuration;
        private static readonly IDictionary<Type, object> handlers = new Dictionary<Type, object>();

        public Bus(IPaymentServiceConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Subscribe<T>(Action<T> handler)
        {
            handlers.Add(typeof(T), handler);
        }

        public void Publish<T>(T message)
        {
            Console.WriteLine("Published: '{0}'", message);
            ((Action<T>) handlers[typeof (T)])(message);
        }

        public void Dispose()
        {
            Console.Out.WriteLine("Bus Disposed");
        }
    }
}