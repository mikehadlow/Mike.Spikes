using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.Threading
{
    public class ExceptionHandlingInThreads
    {
        public void UncaughtThreadException()
        {
            // uncaught exceptions in threads generally terminate the process.
            // (this wasn't true in earlier framework versions)
            try
            {
                var thread = new Thread(() =>
                    {
                        throw new Exception("Something's gone wrong!");
                    })
                    {
                        Name = "The thread that throws"
                    };
                thread.Start();
                thread.Join();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was thrown: {0}", e.Message);
            }
        }

        public void CaughtThreadException()
        {
            // you should generally wrap the top of each thread stack with a try.. catch.. handler
            var thread = new Thread(() =>
                {
                    try
                    {
                        throw new Exception("Something's gone wrong!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception was thrown: {0}", e.Message);
                    }
                })
                {
                    Name = "Thread that catches"
                };
            thread.Start();
            thread.Join();
        }

        public void TplExceptionHandling()
        {
            var task = Task.Factory.StartNew(() =>
                {
                    throw new Exception("Something's gone wrong!");
                });

            try
            {
                // exception is rethrown on the thread where the thread is joined.
                task.Wait();
            }
            catch (Exception e)
            {
                // note e.InnerException
                Console.WriteLine("An exception was thrown: {0}", e.InnerException.Message);
            }
        }
    }
}