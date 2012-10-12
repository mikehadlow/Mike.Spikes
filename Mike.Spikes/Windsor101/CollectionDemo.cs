using System.Linq;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mike.Spikes.Windsor101
{
    public interface IProcessStep
    {
        void Process(Context context);
    }

    public class ProcessStep1 : IProcessStep
    {
        public void Process(Context context)
        {
            context.Text += " Process One.";
        }
    }

    public class ProcessStep2 : IProcessStep
    {
        public void Process(Context context)
        {
            context.Text += " Process Two.";
        }
    }

    public class ProcessStep3 : IProcessStep
    {
        public void Process(Context context)
        {
            context.Text += " Process Three.";
        }
    }

    public class Context
    {
        public Context()
        {
            Text = "Initial Value.";
        }

        public string Text { get; set; }
    }

    public interface IProcessor
    {
        void Process(Context context);
    }

    public class Processor : IProcessor
    {
        private readonly IEnumerable<IProcessStep> processSteps;

        public Processor(IEnumerable<IProcessStep> processSteps)
        {
            this.processSteps = processSteps;
        }

        public void Process(Context context)
        {
            foreach (var processStep in processSteps)
            {
                processStep.Process(context);
            }
        }
    }

    public class ArrayDemoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, allowEmptyCollections:true));

            container.Register(
                    Classes.FromThisAssembly().BasedOn<IProcessStep>().WithService.FirstInterface().LifestyleTransient(),
                    Component.For<IProcessor>().ImplementedBy<Processor>()
                );
        }
    }
}