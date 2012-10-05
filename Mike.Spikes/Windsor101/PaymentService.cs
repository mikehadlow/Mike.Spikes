namespace Mike.Spikes.Windsor101
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBus bus;

        public PaymentService(IAccountRepository accountRepository, IBus bus)
        {
            this.accountRepository = accountRepository;
            this.bus = bus;
        }

        public void Start()
        {
            bus.Subscribe<PayCustomer>(PayCustomerHandler);
        }

        public void PayCustomerHandler(PayCustomer payCustomer)
        {
            var account = accountRepository.GetAccountByCustomerId(payCustomer.CustomerId);
            account.MakePayment(payCustomer.Amount);
        }
    }
}