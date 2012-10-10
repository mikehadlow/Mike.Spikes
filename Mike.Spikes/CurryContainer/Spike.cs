using System;

namespace Mike.Spikes.CurryContainer
{
    public class Spike
    {
        public void Spike1()
        {
            var registration = new Container()
                .Register<Func<Input, Data>, Func<Data, Data>, int, Data>(Module.GetAndTransform)
                .Register<Input,Data>(Module.DataAccsessor)
                .Register<Data,Data>(Module.Transformer);

            var main = registration.Get<Func<int, Data>>();

            var data = main(10);

            Console.Out.WriteLine("data.Id = {0}", data.Id);
            Console.Out.WriteLine("data.Name = {0}", data.Name);
        }

        public void Spike2()
        {
            var id = 10;
            var data = Module.GetAndTransform(Module.DataAccsessor, Module.Transformer, id);

            Console.Out.WriteLine("data.Id = {0}", data.Id);
            Console.Out.WriteLine("data.Name = {0}", data.Name);
        }
    }

    public class Input
    {
        public int Id { get; set; }
    }

    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class Module
    {
        public static Data GetAndTransform(Func<Input,Data> dataAccsessor, Func<Data,Data> transformer, int id)
        {
            var input = new Input() {Id = id};
            var data = dataAccsessor(input);
            var transformed = transformer(data);
            return transformed;
        }

        public static Data DataAccsessor(Input input)
        {
            return new Data
            {
                Id = input.Id,
                Name = "Test"
            };
        }

        public static Data Transformer(Data original)
        {
            original.Name = original.Name + " transformed";
            return original;
        }
    }
}