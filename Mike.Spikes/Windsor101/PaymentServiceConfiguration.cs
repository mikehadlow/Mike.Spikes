namespace Mike.Spikes.Windsor101
{
    public class PaymentServiceConfiguration : IPaymentServiceConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string RabbitMqConnectionString { get; set; }
    }
}