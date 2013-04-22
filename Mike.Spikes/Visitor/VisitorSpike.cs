using System;
using System.Collections.Generic;

namespace Mike.Spikes.Visitor
{
    public class VisitorSpike
    {
        public void Test()
        {
            var characters = new List<ICharacter>
            {
                new CatInTheHat(),
                new Fish(),
                new Thing()
            };

            var visitor = new FishVisitor();

            foreach (var character in characters)
            {
                visitor.Visit((dynamic)character);
            }
        }     
    }

    public class FishVisitor
    {
        public void Visit(Fish fish)
        {
            Console.Out.WriteLine("fish");
        }

        public void Visit(CatInTheHat character)
        {
            Console.Out.WriteLine("cat");
        }
    }

    public interface ICharacter {}

    public class Fish : ICharacter
    {
    }

    public class CatInTheHat : ICharacter
    {
    }

    public class Thing : ICharacter
    {
    }
}