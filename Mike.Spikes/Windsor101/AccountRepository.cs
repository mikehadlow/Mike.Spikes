using System;

namespace Mike.Spikes.Windsor101
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private IPaymentServiceConfiguration configuration;

        public AccountRepository(IPaymentServiceConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Account GetAccountByCustomerId(int customerId)
        {
            return new Account
            {
                CustomerId = customerId,
                Balance = 100M
            };
        }

        public void Dispose()
        {
            Console.Out.WriteLine("AccountRepository Disposed");
        }
    }
}