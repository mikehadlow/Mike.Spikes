using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IPaymentServiceConfiguration>().ImplementedBy<PaymentServiceConfiguration>().LifeStyle.Singleton,
                Component.For<IPaymentService>().ImplementedBy<PaymentService>().LifeStyle.Transient,
                Component.For<IBus>().ImplementedBy<Bus>().LifeStyle.Transient,
                Component.For<IAccountRepository>().ImplementedBy<AccountRepository>().LifeStyle.Transient,
                Component.For<Now>().Instance(() => DateTime.UtcNow)
                );
        }
    }
}