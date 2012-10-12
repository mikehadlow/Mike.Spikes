using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public class TypeFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<IPaymentServiceFactory>().AsFactory(),
                Component.For<PaymentServiceUser>()
                );
        }
    }

    public interface IPaymentServiceFactory : IDisposable
    {
        IPaymentService Create();
        void Release(IPaymentService paymentService);
    }

    public class PaymentServiceUser
    {
        private readonly IPaymentServiceFactory paymentServiceFactory;

        public PaymentServiceUser(IPaymentServiceFactory paymentServiceFactory)
        {
            this.paymentServiceFactory = paymentServiceFactory;
        }

        public void UsePaymentService()
        {
            var paymentService = paymentServiceFactory.Create();

            paymentService.Start();
            Program.PretendToPublish();

            paymentServiceFactory.Release(paymentService);

            paymentServiceFactory.Dispose();
        }
    }
}