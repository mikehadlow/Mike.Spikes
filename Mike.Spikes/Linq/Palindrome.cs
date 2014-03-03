namespace Mike.Spikes.Linq
{
    public static class Palindrome
    {
        public static bool IsPalindrome(this int x)
        {
            var n = x;
            var r = 0;
            while (n > 0)
            {
                var d = n % 10;
                r = r * 10 + d;
                n = n / 10;
            }
            return r == x;
        }
    }
}