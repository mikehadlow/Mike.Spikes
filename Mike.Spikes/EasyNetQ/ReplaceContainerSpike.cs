using System;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;
using IServiceProvider = EasyNetQ.IServiceProvider;

namespace Mike.Spikes.EasyNetQ
{
    public class ReplaceContainerSpike
    {
        public void DoIt()
        {
            // register our alternative container factory
            RabbitHutch.SetContainerFactory(() =>
                {
                    // create an instance of Windsor
                    var windsorContainer = new WindsorContainer();
                    // wrap it in our implementation of EasyNetQ.IContainer
                    return new WindsorContainerWrapper(windsorContainer);
                });
            // now we can create an IBus instance, but it's resolved from
            // windsor, rather than EasyNetQ's default service provider.
            var bus = RabbitHutch.CreateBus("host=localhost");

            var are = new AutoResetEvent(false);

            bus.Subscribe<MyMessage>("replace_container_spike", message =>
                {
                    Console.WriteLine(message.Text);
                    are.Set();
                });

            bus.Publish(new MyMessage{ Text = "Hello with Windsor!"} );

            are.WaitOne(1000);

            ((WindsorContainerWrapper)bus.Advanced.Container).Dispose();
            bus.Dispose();
        }
    }

    public class MyMessage
    {
        public string Text { get; set; }
    }

    public class WindsorContainerWrapper : IContainer, IDisposable
    {
        private readonly IWindsorContainer windsorContainer;

        public WindsorContainerWrapper(IWindsorContainer windsorContainer)
        {
            this.windsorContainer = windsorContainer;
        }

        public TService Resolve<TService>() where TService : class
        {
            return windsorContainer.Resolve<TService>();
        }

        public IServiceRegister Register<TService>(System.Func<IServiceProvider, TService> serviceCreator) 
            where TService : class
        {
            windsorContainer.Register(
                Component.For<TService>().UsingFactoryMethod(() => serviceCreator(this)).LifeStyle.Singleton
                );
            return this;
        }

        public IServiceRegister Register<TService, TImplementation>() 
            where TService : class 
            where TImplementation : class, TService
        {
            windsorContainer.Register(
                Component.For<TService>().ImplementedBy<TImplementation>().LifeStyle.Singleton
                );
            return this;
        }

        public void Dispose()
        {
            windsorContainer.Dispose();
        }
    }
}