using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Mike.Spikes.ObservablePlay
{
    public interface IBus : IDisposable, IObservable<string>
    {
        void Publish(string message);
    }

    public class RabbitObservable : IBus
    {
        private readonly string queue;
        private readonly IConnection connection;

        public static RabbitObservable OpenConnection(string amqpConnection)
        {
            var factory = new ConnectionFactory
                {
                    Uri = amqpConnection
                };
            return new RabbitObservable(factory.CreateConnection());
        }

        private RabbitObservable(IConnection connection)
        {
            this.connection = connection;
            using (var model = connection.CreateModel())
            {
                queue = model.QueueDeclare();
            }
        }

        public void Dispose()
        {
            connection.Close();
        }

        public void Publish(string message)
        {
            using (var model = connection.CreateModel())
            {
                var properties = model.CreateBasicProperties();
                var body = Encoding.UTF8.GetBytes(message);
                model.BasicPublish("", queue, properties, body);
            }
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            var disposed = false;
            new System.Threading.Thread(state =>
                {
                    var model = connection.CreateModel();
                    var subscription = new RabbitMQ.Client.MessagePatterns.Subscription(model, queue, false);
                    while(!disposed)
                    {
                        var deliverEventArgs = subscription.Next();

                        if (deliverEventArgs != null)
                        {
                            var message = Encoding.UTF8.GetString(deliverEventArgs.Body);
                            
                            Log.WriteLine("Before running observer");
                            observer.OnNext(message);
                            Log.WriteLine("After running observer");

                            subscription.Ack(deliverEventArgs);
                        }
                    }
                }) { Name = "Subscription" }.Start();

            return Disposable.Create(() =>
                {
                    disposed = true;
                    observer.OnCompleted();
                });
        }
    }

    public class Tests
    {
        public void Spike()
        {
            var are = new System.Threading.AutoResetEvent(false);
            using (var bus = RabbitObservable.OpenConnection("amqp://localhost"))
            {
                var subscription = bus.Subscribe(message =>
                    {
                        Log.WriteLine("---- Got message '{0}'", message);
                        System.Threading.Thread.Sleep(10);
                        Log.WriteLine("---- Completed message processing");
                        are.Set();
                    });

                bus.Publish("Hello!");
                Log.WriteLine("Published");

                are.WaitOne(1000);
                subscription.Dispose();
            }
        }
    }
}