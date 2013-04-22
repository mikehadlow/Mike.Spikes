using System;

namespace Mike.Spikes.FluentApi
{
    public class FluentApiSpike
    {
        public void TryIt()
        {
            var thing = new Thing();
            thing.Do();
        }     
    }

    public class Thing
    {
        public DoConfig Do()
        {
            Console.WriteLine("Done");
            return new DoConfig();
        }
    }

    public class DoConfig
    {
        public DoConfig WithName(string name)
        {
            return this;
        }   
    }
}