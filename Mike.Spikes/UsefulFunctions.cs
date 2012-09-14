using System;

namespace Mike.Spikes
{
    public static class UsefulFunctions
    {
        public static Person CreateAPerson()
        {
            var person = new Person
            {
                FirstName = "Leo",
                LastName = "Hadlow",
                Age = 10,
                Address = new Address
                {
                    StreetAddress = "4 Chelston Avenue",
                    City = "Brighton & Hove",
                    State = "East Sussex",
                    PostalCode = "BN3 5SR"
                },
                PhoneNumbers =
                    {
                        new PhoneNumber
                        {
                            Type = PhoneNumberType.Home,
                            Number = "01273 421444"
                        },
                        new PhoneNumber
                        {
                            Type = PhoneNumberType.Mobile,
                            Number = "07866 456123"
                        }
                    }
            };
            return person;
        }

        public static void WritePersonToConsole(Person person)
        {
            Console.Out.WriteLine("{0}", person.FirstName);
            Console.Out.WriteLine("{0}", person.LastName);
            Console.Out.WriteLine("{0}", person.Age);

            Console.Out.WriteLine("\nAddress:");
            Console.Out.WriteLine("\t{0}", person.Address.StreetAddress);
            Console.Out.WriteLine("\t{0}", person.Address.City);
            Console.Out.WriteLine("\t{0}", person.Address.State);
            Console.Out.WriteLine("\t{0}", person.Address.PostalCode);

            foreach (var phoneNumber in person.PhoneNumbers)
            {
                Console.Out.WriteLine("\nPhone:");
                Console.Out.WriteLine("\t{0}", phoneNumber.Type);
                Console.Out.WriteLine("\t{0}", phoneNumber.Number);
            }
        }
    }
}