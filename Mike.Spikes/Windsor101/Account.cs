using System;

namespace Mike.Spikes.Windsor101
{
    public class Account
    {
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastTransaction { get; set; }

        public void MakePayment(decimal amount, DateTime transactionDate)
        {
            Balance += amount;
            LastTransaction = transactionDate;
            Console.Out.WriteLine("Made payment on {0} balance is now: £{1:#,##0.00}", transactionDate, Balance);
        }

        public override string ToString()
        {
            return string.Format("Account. Balance: {0:#,##0.00}. Last Transaction: {1}", Balance, LastTransaction);
        }
    }
}