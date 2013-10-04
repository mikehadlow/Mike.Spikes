using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.ProducerConsumer
{
    public class ProducerConsumerUrlGetter
    {
        public void Start()
        {
            var ventilatorQueue = new BlockingCollection<GetPageMessage>();
            var sinkQueue = new BlockingCollection<PageResultMessage>();
            var allTasks = new List<Task>();
            var workerTasks = new List<Task>();

            allTasks.Add(StartSink(sinkQueue));

            for (var i = 0; i < 3; i++)
            {
                var index = i;
                workerTasks.Add(StartWorker(index, ventilatorQueue, sinkQueue));    
            }

            allTasks.Add(Task.Factory.ContinueWhenAll(workerTasks.ToArray(), _ => sinkQueue.CompleteAdding()));

            allTasks.Add(StartVentilator(ventilatorQueue).ContinueWith(_ => ventilatorQueue.CompleteAdding()));

            Task.WaitAll(allTasks.ToArray());
        }

        public static Task StartVentilator(BlockingCollection<GetPageMessage> ventilatorQueue)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var url in UrlList.Urls)
                {
                    ventilatorQueue.Add(new GetPageMessage { Url = url });
                }
            }, TaskCreationOptions.LongRunning);
        }

        public static Task StartWorker(int workerNumber,
            BlockingCollection<GetPageMessage> ventilatorQueue,
            BlockingCollection<PageResultMessage> sinkQueue)
        {
            return Task.Factory.StartNew(() =>
            {
                var client = new WebClient();
                var stopwatch = new Stopwatch();
                foreach (var workItem in ventilatorQueue.GetConsumingEnumerable())
                {
                    stopwatch.Reset();
                    stopwatch.Start();
                    var page = client.DownloadString(workItem.Url);
                    stopwatch.Stop();
                    sinkQueue.Add(new PageResultMessage
                        {
                            Size = page.Length,
                            Milliseconds = stopwatch.ElapsedMilliseconds,
                            Url = workItem.Url,
                            ThreadId = Thread.CurrentThread.ManagedThreadId
                        });
                }
            }, TaskCreationOptions.LongRunning);
        }

        public static Task StartSink(BlockingCollection<PageResultMessage> sinkQueue)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var workItem in sinkQueue.GetConsumingEnumerable())
                {
                    Console.WriteLine("Thread:\t{3}, Time:\t{0},\tSize {1},\tUrl {2}", 
                        workItem.Milliseconds, workItem.Size, workItem.Url, workItem.ThreadId);
                }
            }, TaskCreationOptions.LongRunning);
        }
    }

    public class GetPageMessage
    {
        public string Url { get; set; }
    }

    public class PageResultMessage
    {
        public string Url { get; set; }
        public long Milliseconds { get; set; }
        public long Size { get; set; }
        public long ThreadId { get; set; }
    }
}