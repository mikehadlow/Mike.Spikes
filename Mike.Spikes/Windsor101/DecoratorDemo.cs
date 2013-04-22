using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public interface IMyDataAccess
    {
        Account GetAccountById(int id);
    }

    public class MyDataAccess : IMyDataAccess
    {
        public Account GetAccountById(int id)
        {
            return new Account
            {
                Balance = 100M,
                CustomerId = id,
                LastTransaction = DateTime.Now
            };
        }
    }

    public class MyDataAccessLogger : IMyDataAccess
    {
        private readonly IMyDataAccess myDataAccess;

        public MyDataAccessLogger(IMyDataAccess myDataAccess)
        {
            this.myDataAccess = myDataAccess;
        }

        public Account GetAccountById(int id)
        {
            // you a chance to examine and alter the input before the decorated instance is invoked.
            Console.Out.WriteLine("About to get account with id {0}", id);

            var account = myDataAccess.GetAccountById(id);
            
            // you also get a chance to examine the output after the decorated instance is invoked.
            Console.Out.WriteLine("Got account with Id {0}, balance, {1:0.00}", id, account.Balance);

            return account;
        }
    }

    public class InterestPaymentProcessor : IMyDataAccess
    {
        private readonly IMyDataAccess myDataAccess;

        public InterestPaymentProcessor(IMyDataAccess myDataAccess)
        {
            this.myDataAccess = myDataAccess;
        }

        public Account GetAccountById(int id)
        {
            var account = myDataAccess.GetAccountById(id);

            account.MakePayment(account.Balance * 0.01M, DateTime.Now);

            return account;
        }
    }

    public class DecoratorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMyDataAccess>().ImplementedBy<MyDataAccessLogger>(),
                Component.For<IMyDataAccess>().ImplementedBy<InterestPaymentProcessor>(),
                Component.For<IMyDataAccess>().ImplementedBy<MyDataAccess>()
                );
        }
    }
}