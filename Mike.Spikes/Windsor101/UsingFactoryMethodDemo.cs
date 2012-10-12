using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public class UsingFactoryMethodInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ISomeService>().UsingFactoryMethod(ServiceFactory.CreateSomeService)
                );
        }
    }

    public interface ISomeService
    {
        void DoSomething();
    }

    public class SomeService : ISomeService
    {
        public void DoSomething()
        {
            Console.Out.WriteLine("I've done something");
        }
    }

    public static class ServiceFactory
    {
        public static ISomeService CreateSomeService()
        {
            return new SomeService();
        }
    }
}