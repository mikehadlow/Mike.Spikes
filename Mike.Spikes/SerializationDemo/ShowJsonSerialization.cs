using System;
using System.IO;
using Newtonsoft.Json;

namespace Mike.Spikes
{
    public class ShowJsonSerialization
    {
        public void Deserialize()
        {
            var personAsJson = File.ReadAllText("Person.json");

            var person = JsonConvert.DeserializeObject<Person>(personAsJson);

            UsefulFunctions.WritePersonToConsole(person);
        }

        public void Serialize()
        {
            var person = UsefulFunctions.CreateAPerson();

            var personAsJson = JsonConvert.SerializeObject(person, Formatting.Indented);

            Console.Out.WriteLine(personAsJson);
        }
    }
}