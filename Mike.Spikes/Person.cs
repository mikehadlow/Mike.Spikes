using System.Collections.Generic;

namespace Mike.Spikes
{
    public class Person
    {
        public Person()
        {
            PhoneNumbers = new List<PhoneNumber>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Address Address { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    public class PhoneNumber
    {
        public PhoneNumberType Type { get; set; }
        public string Number { get; set; }
    }

    public enum PhoneNumberType
    {
        Home,
        Fax,
        Mobile,
        Work
    }
}