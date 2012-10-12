using System;
using System.Dynamic;

using ImpromptuInterface;
using ImpromptuInterface.Dynamic;

namespace Mike.Spikes.Dynamic
{
    public class ExpandoObjectSpike
    {
        public void Spike()
        {
            dynamic expandoObject = new ExpandoObject();
            expandoObject.Name = "Mike";
            expandoObject.Dob = new DateTime(1965, 1, 7);
            expandoObject.GetAge = (Func<TimeSpan>) (() => DateTime.Now.Subtract(expandoObject.Dob));

            IPerson person = Impromptu.ActLike(expandoObject);

            Console.Out.WriteLine("person.Name = {0}", person.Name);
            Console.Out.WriteLine("person.Age = {0}", person.Dob);
            Console.Out.WriteLine("person.GetAge() = {0}", person.GetAge().TotalDays / 365);
        }     
    }

    public interface IPerson
    {
        string Name { get; }
        DateTime Dob { get; }
        TimeSpan GetAge();
    }
}