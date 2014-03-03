using System;
using System.Linq.Expressions;

namespace Mike.Spikes.Linq
{
    public class ExpressionTrees
    {
        public void CreatingExpressionWithApi()
        {
            var three = Expression.Constant(3);
            var x = Expression.Parameter(typeof (int), "x");
            var add = Expression.Add(x, three);

            var func = Expression.Lambda<Func<int, int>>(add, new[]{x});

            Console.Out.WriteLine(func);

            var compiledFunc = func.Compile();

            var result = compiledFunc(10);
            Console.Out.WriteLine("result = {0}", result);
        }

        public void CreatingExpressionViaLambda()
        {
            Expression<Func<int, int>> func = x => x + 3;

            Console.Out.WriteLine(func);

            var compiledFunc = func.Compile();

            var result = compiledFunc(10);
            Console.Out.WriteLine("result = {0}", result);
        }

        // can't create a multi line expression using a lambda
        public void CreatingMultiLineExpression()
        {
//            Expression<Func<int, int>> func = x =>
//                {
//                    var m = x + 2;
//                    var n = m*2;
//                    return n;
//                };
        }

        // but can create multi-line lambda expressions with the API.

        public void CreatingExpressionWhichCallsMethod()
        {
            Expression<Func<int, int>> func = x => DoSomething(x);

            // the question is what happens when your query provider sees a user
            // function?
            Console.Out.WriteLine(func);
        }

        public static int DoSomething(int x)
        {
            return x*2;
        }

        public static void DemoWritingAMethodThatTakesAnExpression()
        {
            DoSomethingWithExpression(x => x == 1);
        }

        public static void DoSomethingWithExpression(Expression<Func<int, bool>> expression)
        {
            var body = expression.Body as BinaryExpression;
            if (body != null)
            {
                Console.Out.WriteLine("It's a binary expression");
            }
        }
    }
}