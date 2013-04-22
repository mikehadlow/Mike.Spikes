using System;
using System.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace Mike.Spikes.HttpHandler
{
    public class MyHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var service = new MyService(new MyDependency());
            service.ProcessRequest(new HttpContextWrapper(context));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }

    public class MyService
    {
        private readonly IMyDependency myDependency;

        public MyService(IMyDependency myDependency)
        {
            this.myDependency = myDependency;
        }

        public void ProcessRequest(HttpContextBase httpContext)
        {
            myDependency.DoSomething();
        }
    }

    public interface IMyDependency
    {
        void DoSomething();
    }

    public class MyDependency : IMyDependency
    {
        public void DoSomething()
        {
            // do something here
        }
    }

    [TestFixture]
    public class MyHttpHandlerTests
    {
        private MyService service;
        private IMyDependency myDependency;

        [SetUp]
        public void SetUp()
        {
            myDependency = MockRepository.GenerateStub<IMyDependency>();
            service = new MyService(myDependency);
        }

        [Test]
        public void Should_call_DoSomething()
        {
            service.ProcessRequest(new MockHttpContext());
            myDependency.AssertWasCalled(x => x.DoSomething());
        }
    }

    public class MockHttpContext : HttpContextBase
    {
        
    }
}