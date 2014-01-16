namespace Mike.Spikes.ExceptionHandling
{
    public class StructuredExceptionHandling
    {
        public void DoIt()
        {
            First();
        }

        public void First()
        {
            Second();
        }

        public void Second()
        {
            Third();
        }

        public void Third()
        {
            throw new ExceptionHandlingDemoException();
        }
    }
}