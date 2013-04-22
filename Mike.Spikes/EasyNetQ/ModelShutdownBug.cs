using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;

namespace Mike.Spikes.EasyNetQ
{
    public class ModelShutdownBug
    {
        private const string exName = "my_exchange";
        private const string hostName = "localhost";

        public void Test()
        {
            var resetEvent = new AutoResetEvent(false);

            var bus = RabbitHutch.CreateBus(String.Format("host={0}", hostName)).Advanced;
            var exchange = Exchange.DeclareFanout(exName);
            var queue = Queue.DeclareTransient();

            queue.BindTo(exchange, "_");
            bus.Subscribe(queue, (msg, properties, messageReceivedInfo) => 
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine(Encoding.UTF8.GetString(msg));
                    resetEvent.Set();
                }));

            using (var channel = bus.OpenPublishChannel())
            {
                channel.Publish(exchange, "", new MessageProperties(), Encoding.UTF8.GetBytes("Hello World"));
            }

            resetEvent.WaitOne(TimeSpan.FromSeconds(5));

            bus.Dispose();
        } 
    }
}