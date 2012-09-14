using System;
using System.IO;
using System.Xml.Serialization;

namespace Mike.Spikes
{
    public class ShowXmlSerialization
    {
        public void Deserialize()
        {
            var serializer = new XmlSerializer(typeof(Person));

            var person = (Person)serializer.Deserialize(File.OpenRead("Person.xml"));

            UsefulFunctions.WritePersonToConsole(person);
        }

        public void Serialize()
        {
            var person = UsefulFunctions.CreateAPerson();

            var serializer = new XmlSerializer(typeof (Person));

            var writer = new StringWriter();
            serializer.Serialize(writer, person);
            Console.Out.WriteLine(writer.ToString());
        }
    }
}