using System;
using System.Threading;
using EasyNetQ;

namespace Mike.Spikes
{
    public class HowToBlockUntilAMessageArrives
    {
        public void ServiceStartsWithTimelyResponse()
        {
            var bus = RabbitHutch.CreateBus("host=localhost");

            // this is the service that sends the data. It waits for a data request and then
            // publishes some data
            bus.Subscribe<RequestDataMessage>("data_service", _ =>
            {
                bus.Publish(new DataMessage{ Text = "The message full of data" });
            });

            var myService = new MyService(bus);

            myService.Start();

            // should expect to see "Got text: 'The message full of data'"
            myService.Process();

            myService.Dispose();
            bus.Dispose();
        }

        public void ServiceStartsButNoDataArrives()
        {
            var bus = RabbitHutch.CreateBus("host=localhost");

            // this is the service that sends the data. It waits for a data request and then
            // publishes some data
            bus.Subscribe<RequestDataMessage>("data_service", _ =>
            {
                // don't send any data
            });

            var myService = new MyService(bus);

            myService.Start();

            // should expect to see "Timeout occured"
            myService.Process();

            myService.Dispose();
            bus.Dispose();
        }
    }

    /// <summary>
    /// Shows how you can block processing until a message arrives, or until a timeout fires
    /// </summary>
    public class MyService : IDisposable
    {
        private readonly IBus bus;
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private string text;
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);

        public MyService(IBus bus)
        {
            this.bus = bus;
        }

        /// <summary>
        /// Start fires when the service starts (you might also put this code in the constructor)
        /// </summary>
        public void Start()
        {
            bus.Publish(new RequestDataMessage());

            bus.Subscribe<DataMessage>("mike_spike_data_processor", OnDataAvailable);
        }

        public void OnDataAvailable(DataMessage data)
        {
            text = data.Text;

            // signal the autoResetEvent so the processor is allowed to proceed.
            autoResetEvent.Set();
        }

        /// <summary>
        /// Do some processing
        /// </summary>
        public void Process()
        {
            // wait until data is available, or timeout occurs
            autoResetEvent.WaitOne(timeout);

            if(text == null)
            {
                Console.WriteLine("Timeout occured");
            }
            else
            {
                Console.WriteLine("Got text: '{0}'", text);
            }
        }

        public void Dispose()
        {
            autoResetEvent.Dispose();
        }
    }

    public class RequestDataMessage
    {
        
    }

    public class DataMessage
    {
        public string Text { get; set; }
    }
}