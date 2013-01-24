using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.ProducerConsumer
{
    public class ProducerConsumerSpike
    {
        public void Start()
        {
            var ventilatorQueue = new BlockingCollection<WorkItem>();
            var sinkQueue = new BlockingCollection<WorkItem>();

            StartSink(sinkQueue);

            StartWorker(0, ventilatorQueue, sinkQueue);
            StartWorker(1, ventilatorQueue, sinkQueue);
            StartWorker(2, ventilatorQueue, sinkQueue);

            StartVentilator(ventilatorQueue);

            Thread.Sleep(1000);
            ventilatorQueue.CompleteAdding();
            sinkQueue.CompleteAdding();
            ventilatorQueue.Dispose();
            sinkQueue.Dispose();
        }

        public static void StartVentilator(BlockingCollection<WorkItem> ventilatorQueue)
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    ventilatorQueue.Add(new WorkItem { Text = string.Format("Item {0}", i) });
                }
            }, TaskCreationOptions.LongRunning);
        }

        public static void StartWorker(int workerNumber,
            BlockingCollection<WorkItem> ventilatorQueue,
            BlockingCollection<WorkItem> sinkQueue)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var workItem in ventilatorQueue.GetConsumingEnumerable())
                {
                    // pretend to take some time to process
                    Thread.Sleep(10);
                    workItem.Text = workItem.Text + " processed by worker " + workerNumber;
                    sinkQueue.Add(workItem);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public static void StartSink(BlockingCollection<WorkItem> sinkQueue)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var workItem in sinkQueue.GetConsumingEnumerable())
                {
                    Console.WriteLine("Processed Messsage: {0}", workItem.Text);
                }
            }, TaskCreationOptions.LongRunning);
        }
    }

    public class WorkItem
    {
        public string Text { get; set; }
    }
}