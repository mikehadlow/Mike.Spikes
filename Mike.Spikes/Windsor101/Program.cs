using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Mike.Spikes.Windsor101
{
    public class Program
    {
        public void WithManualWireUp()
        {
            var configuration = new PaymentServiceConfiguration();
            var bus = new Bus(configuration);
            var accountRepository = new AccountRepository(configuration);
            var paymentService = new PaymentService(accountRepository, bus, () => DateTime.Now);

            paymentService.Start();

            // pretend something is publishing to me
            PretendToPublish();

            // wait for some shutdown event here
            bus.Dispose();
            accountRepository.Dispose();
        }

        public void WithWidsorWireUp()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var paymentService = container.Resolve<IPaymentService>();

            paymentService.Start();

            // pretend something is publishing to me
            PretendToPublish();

            // wait for some shutdown event here
            container.Release(paymentService);

            Console.Out.WriteLine("disposing container");
            container.Dispose();
        }

        public static void PretendToPublish()
        {
            new Bus(new PaymentServiceConfiguration()).Publish(new PayCustomer { Amount = 20M, CustomerId = 101 });
        }
    }
}