using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public interface IDemoConfiguration
    {
        string ConnectionString { get; }
        int PollingInterval { get; }
    }

    public class DemoConfiguration : IDemoConfiguration
    {
        public DemoConfiguration(string connectionString, int pollingInterval)
        {
            ConnectionString = connectionString;
            PollingInterval = pollingInterval;
        }

        public string ConnectionString { get; private set; }
        public int PollingInterval { get; private set; }
    }

    public class DemoConfigurationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDemoConfiguration>().ImplementedBy<DemoConfiguration>().Named("configuration")
                );
        }
    }
}