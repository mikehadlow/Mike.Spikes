using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace Mike.Spikes.ProducerConsumer
{
    public class AsyncUrlGetter
    {
        public void Start()
        {
            var tasks = UrlList.Urls.Select(DownloadStringAsync);

            var allTasks = Task.WhenAll(tasks);
            allTasks.Wait();

            foreach (var pageResult in allTasks.Result)
            {
                Console.Out.WriteLine("Time: {0}\tSize: {1}\tUrl: {2}", 
                    pageResult.Milliseconds, pageResult.Size, pageResult.Url);
            }
        }

        public Task<PageResult> DownloadStringAsync(string url)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var request = WebRequest.CreateHttp(url);
            request.Timeout = 60*1000;
            var task = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
            return task
                .ContinueWith(task1 =>
                    {
                        if (task1.IsFaulted)
                        {
                            return Task.Factory.StartNew(() => new PageResult
                                {
                                    Url = url,
                                    Milliseconds = 0,
                                    Size = 0
                                });
                        }

                        var page = StringFromStream(task1.Result.GetResponseStream()).ContinueWith(task2 =>
                        {
                            Console.Out.WriteLine("Got result for {0}", url);
                            task1.Result.Close();
                            stopwatch.Stop();
                            return new PageResult
                            {
                                Url = url,
                                Milliseconds = stopwatch.ElapsedMilliseconds,
                                Size = task2.Result.Length
                            };
                        });
                        return page;
                    })
                .Unwrap();
        }

        public Task<string> StringFromStream(Stream stream)
        {
            try
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                return Task.Factory.StartNew(() => e.Message);
            }
        }
    }

    public class PageResult
    {
        public string Url { get; set; }
        public long Size { get; set; }
        public long Milliseconds { get; set; }
    }
}