using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mike.Spikes.TaskCancellation
{
    public class TaskCancellationSpike
    {
        public void PublisherConfirmsMockup()
        {
            var confirms = new PublisherConfirms();
            var model = new MyModel();
            var dispatcher = new Dispatcher(model);
            var tasks = new List<Task>();

            for (var i = 0; i < 5; i++)
            {
                var task = dispatcher.Invoke(
                    x => confirms.PublishWithConfirm(x,
                        x1 => x1.Publish(new Message {Text = "Hello World!"}))).Unwrap();

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }
    }

    public class PublisherConfirms
    {
        readonly Dictionary<int, Action> dictionary = new Dictionary<int, Action>();
        const int timeout = 500;
        private MyModel cachedModel;

        private void SetModel(MyModel model)
        {
            if (cachedModel == model) return;
            
            cachedModel = model;
            model.SetConfirmsOn();
            model.Confirm += confirmId =>
                {
                    if (!dictionary.ContainsKey(confirmId))
                    {
                        // timed out and removed so just return
                        return;
                    }

                    // handle callback
                    Console.Out.WriteLine("Confirmed {0}", confirmId);
                    dictionary[confirmId]();
                    dictionary.Remove(confirmId);
                };
        }

        public Task PublishWithConfirm(MyModel model, Action<MyModel> publishAction)
        {
            SetModel(model);

            var tcs = new TaskCompletionSource<NullStruct>();
            var publishId = model.GetNextPublishSequenceNumber();
            publishAction(model);
            var timer = new Timer(state =>
            {
                Console.Out.WriteLine("Timed out {0}", publishId);
                dictionary.Remove(publishId);
                tcs.SetException(new TimeoutException());
            }, null, timeout, Timeout.Infinite);

            dictionary.Add(publishId, () =>
            {
                timer.Dispose();
                tcs.SetResult(new NullStruct());
            });

            return tcs.Task;
        }

        private struct NullStruct {}
    }

    public class Dispatcher
    {
        private readonly MyModel model;

        public Dispatcher(MyModel model)
        {
            this.model = model;
        }

        public Task Invoke(Action<MyModel> modelAction)
        {
            var tcs = new TaskCompletionSource<NullStruct>();

            modelAction(model);
            tcs.SetResult(new NullStruct());

            return tcs.Task;
        }

        public Task<T> Invoke<T>(Func<MyModel, T> modelAction)
        {
            var tcs = new TaskCompletionSource<T>();

            var result = modelAction(model);
            tcs.SetResult(result);

            return tcs.Task;
        }

        private struct NullStruct { }
    }

    public class MyModel
    {
        public event Action<int> Confirm;
        private int currentPublishId = 0;
        private readonly Random random = new Random();
        private bool confirmsOn = false;

        public int GetNextPublishSequenceNumber()
        {
            return currentPublishId;
        }

        public void Publish(Message message)
        {
            if (!confirmsOn) return;

            var localPublishId = currentPublishId;
            new Timer(state => OnConfirm((Timer)state, localPublishId)).Change(random.Next(1000), Timeout.Infinite);
            currentPublishId++;
        }

        private void OnConfirm(Timer timer, int publishId)
        {
            timer.Dispose();
            var confirm = Confirm;
            if (confirm != null) confirm(publishId);
        }

        public void SetConfirmsOn()
        {
            confirmsOn = true;
        }
    }

    public class Message
    {
        public string Text { get; set; }
    }
}