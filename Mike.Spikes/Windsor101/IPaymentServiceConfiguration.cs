namespace Mike.Spikes.Windsor101
{
    public interface IPaymentServiceConfiguration
    {
        string DatabaseConnectionString { get; }
        string RabbitMqConnectionString { get; }
    }
}