using System;
using System.IO;
using System.Linq;

namespace Mike.Spikes.Puzzlers
{
    public class Vowels
    {
        private const string path = @"D:\Temp\Words\Words.txt";

        public void FirstAttempt()
        {
            var words = File.ReadAllLines(path).Where(word => word.Intersect("aeiou").Count() == 5);
            foreach (var word in words)
            {
                Console.Out.WriteLine("word = {0}", word);
            }
        }
    }
}