using System;

namespace Mike.Spikes.Windsor101
{
    public interface IBus
    {
        void Subscribe<T>(Action<T> handler);
        void Publish<T>(T message);
    }
}