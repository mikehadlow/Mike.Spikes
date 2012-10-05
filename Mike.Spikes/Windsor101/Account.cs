using System;

namespace Mike.Spikes.Windsor101
{
    public class Account
    {
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public void MakePayment(decimal amount)
        {
            Balance += amount;
            Console.Out.WriteLine("Made payment balance is now: £{0:#,##0.00}", Balance);
        }
    }
}