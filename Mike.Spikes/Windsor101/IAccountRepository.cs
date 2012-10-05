namespace Mike.Spikes.Windsor101
{
    public interface IAccountRepository
    {
        Account GetAccountByCustomerId(int customerId);
    }
}