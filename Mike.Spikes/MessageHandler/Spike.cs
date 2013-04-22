using System;
using System.Collections.Generic;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Mike.Spikes.MessageHandler
{
    public class Spike
    {
        public void TryIt()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            var messageHandler = container.Resolve<MessageHandler>();

            messageHandler.Handle(new MessageOne { Text = "Hello World!" });
            messageHandler.Handle(new MessageTwo { Id = 1001 });
        }
    }

    public class MessageOne
    {
        public string Text { get; set; }
    }

    public class MessageTwo
    {
        public int Id { get; set; }
    }

    public class MessageOneHandler : IHandler<MessageOne>
    {
        public void Handle(MessageOne message)
        {
            Console.Out.WriteLine("message.Text = {0}", message.Text);
        }
    }

    public class MessageTwoHandler : IHandler<MessageTwo>
    {
        public void Handle(MessageTwo message)
        {
            Console.Out.WriteLine("message.Id = {0}", message.Id);
        }
    }

    public class MessageHandlerTwoTheSecond : IHandler<MessageTwo>
    {
        public void Handle(MessageTwo message)
        {
            Console.Out.WriteLine("I'm the second message2 handler message.Id = {0}", message.Id);
        }
    }

    public class MessageHandler
    {
        private readonly IHandlerFactory handlerFactory;

        public MessageHandler(IHandlerFactory handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public void Handle<T>(T message)
        {
            var handlers = handlerFactory.GetHandlerFor<T>();

            foreach (var handler in handlers)
            {
                handler.Handle(message);

                handlerFactory.Release(handler);
            }
        }
    }

    public interface IHandler<in T>
    {
        void Handle(T message);
    }

    public interface IHandlerFactory
    {
        IEnumerable<IHandler<T>> GetHandlerFor<T>();
        void Release<T>(T handler);
    }

    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(
                new CollectionResolver(container.Kernel, allowEmptyCollections: true));

            container.Register(
                    Classes.FromThisAssembly().BasedOn(typeof(IHandler<>)).WithService.FirstInterface().LifestyleTransient(),
                    Component.For<MessageHandler>(),
                    Component.For<IHandlerFactory>().AsFactory()
                );

        }
    }
}