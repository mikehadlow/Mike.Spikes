using System;
using System.Runtime.Serialization;

namespace Mike.Spikes.ExceptionHandling
{
    [Serializable]
    public class ExceptionHandlingDemoException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ExceptionHandlingDemoException()
        {
        }

        public ExceptionHandlingDemoException(string message) : base(message)
        {
        }

        public ExceptionHandlingDemoException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ExceptionHandlingDemoException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}