using System;
using Rhino.Mocks;

namespace Mike.Spikes.Rhino
{
    public class ExtentionMethods
    {
        public void Test()
        {
            // var sut = new ClassToBeExtended();

            var sut = MockRepository.GenerateStub<IInterfaceToBeExtended>();

            sut.DoEx();

            sut.AssertWasCalled(x => x.Do());
        }
    }

    public interface IInterfaceToBeExtended
    {
        void Do();
    }

    public class ClassToBeExtended : IInterfaceToBeExtended
    {
        public void Do()
        {
            Console.WriteLine("Hello from Do");
        }
    }

    public static class Extension
    {
        public static void DoEx(this IInterfaceToBeExtended classToBeExtended)
        {
            classToBeExtended.Do();
        }
    }
}