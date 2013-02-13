using System;

namespace Mike.Spikes.RefactoringExample
{
    public class DataAccess
    {
        public bool Update(string customer, string color, DateTime endDate, out string message)
        {
            Console.Out.WriteLine("Update");
            Console.Out.WriteLine("customer = {0}", customer);
            Console.Out.WriteLine("color = {0}", color);
            Console.Out.WriteLine("endDate = {0}", endDate);

            message = "Entered something in the database";
            return true;
        }

        public bool UpdateTemporaryStore(string customer, string color, out string message)
        {
            Console.Out.WriteLine("UpdateTemporaryStore");
            Console.Out.WriteLine("customer = {0}", customer);
            Console.Out.WriteLine("color = {0}", color);

            message = "Entered something in the database";
            return true;
        }
    }
}