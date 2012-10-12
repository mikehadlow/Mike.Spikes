using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Mike.Spikes.Windsor101
{
    public class TipsAndTricks
    {
        public void XmlConfiguration()
        {
            var container = new WindsorContainer().Install(
                Configuration.FromXmlFile(@"Windsor101\Windsor.config"),
                FromAssembly.This()
                );

            var config = container.Resolve<IDemoConfiguration>();

            Console.Out.WriteLine("config.ConnectionString = {0}", config.ConnectionString);
            Console.Out.WriteLine("config.PollingInterval = {0}", config.PollingInterval);
        }

        public void GenericTypes()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var customerDataAccess = container.Resolve<IDataAccess<Customer>>();
            var customer = customerDataAccess.GetById(101);
            customerDataAccess.Store(customer);

            var employeeDataAccess = container.Resolve<IDataAccess<Employee>>();
            var employee = employeeDataAccess.GetById(44);
            employeeDataAccess.Store(employee);
        }

        public void UsingFactoryMethod()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var someService = container.Resolve<ISomeService>();

            someService.DoSomething();
        }

        public void SupplyingInlineDependencies()
        {
            var container = new WindsorContainer();
            container.Register(
                Component.For<IDemoConfiguration>().ImplementedBy<DemoConfiguration>().DependsOn(new
                {
                    connectionString = "the connection string",
                    pollingInterval = 1234
                }));

            var config = container.Resolve<IDemoConfiguration>();

            Console.Out.WriteLine("config.ConnectionString = {0}", config.ConnectionString);
            Console.Out.WriteLine("config.PollingInterval = {0}", config.PollingInterval);
        }

        public void RegisteringDelegates()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var now = container.Resolve<Func<DateTime>>();
            Console.Out.WriteLine("now() = {0}", now());

            var paymentService = container.Resolve<IPaymentService>();
            paymentService.Start();
            Program.PretendToPublish();
        }     

        public void ResolvingCollections()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var processor = container.Resolve<IProcessor>();

            var context = new Context();
            processor.Process(context);

            Console.Out.WriteLine("context.Text = {0}", context.Text);
        }

        public void Decorators()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var myDataAccess = container.Resolve<IMyDataAccess>();

            var account = myDataAccess.GetAccountById(10);

            Console.Out.WriteLine("Got Account: {0}", account.CustomerId);
        }

        public void Factories()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var paymentServiceUser = container.Resolve<PaymentServiceUser>();

            paymentServiceUser.UsePaymentService();
        }
    }
}