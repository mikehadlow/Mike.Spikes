using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Mike.Spikes.ProducerConsumer
{
    public class DataflowUrlGetter
    {
        public void Start()
        {
            var sink = new ActionBlock<PageResultMessage>((Action<PageResultMessage>)Sink);
            var source = new BufferBlock<GetPageMessage>();
            var linkOptions = new DataflowLinkOptions {PropagateCompletion = false};

            for (int i = 0; i < 10; i++)
            {
                var options = new ExecutionDataflowBlockOptions
                    {
                        BoundedCapacity = 1
                    };
                var worker = new TransformBlock<GetPageMessage, PageResultMessage>(
                    (Func<GetPageMessage, PageResultMessage>)Worker, options);
                source.LinkTo(worker, linkOptions);
                worker.LinkTo(sink, linkOptions);
            }

            foreach (var url in UrlList.Urls)
            {
                source.Post(new GetPageMessage{ Url = url });
            }
            source.Complete();
            sink.Completion.Wait();
        }

        public static void Sink(PageResultMessage message)
        {
            Console.WriteLine("Thread:\t{3}, Time:\t{0},\tSize {1},\tUrl {2}",
                              message.Milliseconds,
                              message.Size,
                              message.Url,
                              message.ThreadId);
        }

        public static PageResultMessage Worker(GetPageMessage message)
        {
            var client = new WebClient();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var page = client.DownloadString(message.Url);
            stopwatch.Stop();
            return new PageResultMessage
            {
                Size = page.Length,
                Milliseconds = stopwatch.ElapsedMilliseconds,
                Url = message.Url,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            };
        }
    }
}