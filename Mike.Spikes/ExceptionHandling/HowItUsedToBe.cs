using System;

namespace Mike.Spikes.ExceptionHandling
{
    public class HowItUsedToBe
    {
        public void ShowMe()
        {
            var fail = TheOldWay(true);
            if (fail != 0)
            {
                Console.Out.WriteLine("Failed with code {0}", fail);
                // deal with failure, maybe return non zero from this function too?
                return;
            }

            Console.Out.WriteLine("Success");
        }

        public int TheOldWay(bool fail)
        {
            if(fail)
            {
                // return an error code
                return 392;
            }

            // success, so return zero
            return 0;
        }
    }
}