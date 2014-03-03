using System;
using System.Linq;
using System.Linq.Expressions;

/*
 * 
 * A DevEd series on Linq
 * 
 * 1. Prerequisites: extension methods, lambda expressions, anonymous types, expression trees.
 * 2. Linq to objects
 * 3. Linq providers
 * 4. Monads!
 * 
 */

namespace Mike.Spikes.Linq
{
    public static class StringExtensions
    {
        public static string l337(this string input)
        {
            return input.Replace('i', '1');
        }        
    }

    public class LinqPrerequisites
    {
        // extension methods
        //  - demo
        //  - unit testing problems
        private void ExtensionMethodDemo()
        {
            var name = "Mike";
            var leeted = name.l337().l337().l337();
            Console.Out.WriteLine("leeted = {0}", leeted);
        }

        // lambda expressions
        //  - delegates "interfaces for functions"
        //  - generic delegates
        //  - demo
        //  - closures
        //  - use in dependency injection
        private void LambdaExpressionDemo(string one, string two)
        {
            Action<string> print = s => Console.Out.WriteLine(one + s + two);

            print("Fred");
            print("Jack");
            print("Sarah");
            print("JOhn");
        }

        private void DoIt()
        {
            LambdaExpressionDemo("hey ", ", how are you?");
        }

        // anonymous types
        //  - type is definted inline with instance
        private void AnonymousTypes()
        {
        }

        // expression trees
        //  - lambda is parsed into an expression tree rather than MSIL
        private void ExpressionTreeDemo()
        {
            Expression<Func<int, int>> add5 = x => x + 5;

            Console.Out.WriteLine(add5);
            Console.Out.WriteLine(add5.Body.NodeType);

            var compiledAdd5 = add5.Compile();

            Console.Out.WriteLine(compiledAdd5(2));
        }
    }
}