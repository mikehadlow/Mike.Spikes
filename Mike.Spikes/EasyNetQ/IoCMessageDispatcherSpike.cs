using System;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;

namespace Mike.Spikes.EasyNetQ
{
    public class IoCMessageDispatcherSpike
    {
        public void ShouldResolveConsumerFromContainer()
        {
            // create the container
            var container = new WindsorContainer();

            // register our consumer
            container.Register(
                Component.For<MyConsumer>().ImplementedBy<MyConsumer>()
                );

            var bus = RabbitHutch.CreateBus("host=localhost");

            // setup the AutoSubscriber
            var autoSubscriber = new AutoSubscriber(bus, "My_subscription_id_prefix")
            {
                MessageDispatcher = new WindsorMessageDispatcher(container)
            };
            autoSubscriber.Subscribe(GetType().Assembly);

            using (var channel = bus.OpenPublishChannel())
            {
                channel.Publish(new MessageA{ Text = "Hello World!" });
            }

            Thread.Sleep(1000);
        }
    }

    public class MessageA
    {
        public string Text { get; set; }
    }

    public class MyConsumer : IConsume<MessageA>
    {
        public void Consume(MessageA message)
        {
            Console.Out.WriteLine("Consumed message: '{0}'", message.Text);
        }
    }

    public class WindsorMessageDispatcher : IMessageDispatcher
    {
        private readonly IWindsorContainer container;

        public WindsorMessageDispatcher(IWindsorContainer container)
        {
            this.container = container;
        }

        public void Dispatch<TMessage, TConsumer>(TMessage message) where TMessage : class where TConsumer : IConsume<TMessage>
        {
            var consumer = (IConsume<TMessage>)container.Resolve<TConsumer>();
            try
            {
                consumer.Consume(message);
            }
            finally
            {
                container.Release(consumer);
            }
        }
    }
}