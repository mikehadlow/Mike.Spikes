using System;

namespace Mike.Spikes.UnixTime
{
    public class UnixTimeSpike
    {
        private const int currentUnixTime = 1357947299;
        private readonly DateTime now = new DateTime(2013, 01, 11, 23, 34, 59, DateTimeKind.Utc);

        public void convert_current_time_to_unix_time()
        {
            var calculatedUnixTime = UnixTime.FromDateTime(now);

            if (calculatedUnixTime == currentUnixTime)
            {
                Console.Out.WriteLine("OK");
            }
            else
            {
                Console.Out.WriteLine("ERROR");
            }
        }

        public void convert_unix_time_to_current_time()
        {
            var calculatedCurrentTime = UnixTime.ToDateTime(currentUnixTime);

            if (calculatedCurrentTime == now && calculatedCurrentTime.Kind == DateTimeKind.Utc)
            {
                Console.Out.WriteLine("OK");
            }
            else
            {
                Console.Out.WriteLine("ERROR");
            }
        }

        public void get_now_in_unix_time()
        {
            Console.Out.WriteLine(UnixTime.Now);
        }
    }

    public class UnixTime
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long FromDateTime(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return (long)dateTime.Subtract(epoch).TotalSeconds;
            }
            throw new ArgumentException("Input DateTime must be UTC");
        }

        public static DateTime ToDateTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        public static long Now
        {
            get
            {
                return FromDateTime(DateTime.UtcNow);
            }
        }
    }
}