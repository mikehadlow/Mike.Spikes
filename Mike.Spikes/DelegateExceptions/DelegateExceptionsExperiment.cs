using System;
using System.Runtime.Serialization;

namespace Mike.Spikes.DelegateExceptions
{
    public class DelegateExceptionsExperiment
    {
        public static void Start()
        {
            Invoke1(() =>
                {
                    throw new DelegateExperimentException("I am throwing");
                });
        }

        public static void Invoke1(Action action)
        {
            Invoke(action);
        }

        public static void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (DelegateExperimentException exception)
            {
                Console.Out.WriteLine("Exception was caught: \n{0}", exception);
            }
        } 
    }

    [Serializable]
    public class DelegateExperimentException : Exception
    {
        public DelegateExperimentException()
        {
        }

        public DelegateExperimentException(string message) : base(message)
        {
        }

        public DelegateExperimentException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DelegateExperimentException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}