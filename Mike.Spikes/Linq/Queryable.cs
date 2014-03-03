using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mike.Spikes.Linq
{
    public class Queryable
    {
        public void DemoWhere()
        {
            var customers = MyData.Customers.Where(x => x.Name == "Mike");

            Console.Out.WriteLine("--- after query before enumeration ----");

            foreach (var customer in customers)
            {
                Console.Out.WriteLine("Customer.Name = {0}", customer.Name);
            }
        }

        public void DemoMax()
        {
            var id = MyData.Customers.Max(x => x.Id);

            Console.Out.WriteLine("--- after query before enumeration ----");

            Console.Out.WriteLine("Max Id: {0}", id);
        }
    }

    public class MyData
    {
        public static IQueryable<Customer> Customers
        {
            get { return new MyQueryable<Customer>(new MyQueryProvider());}
        } 
    }

    public class MyQueryable<T> : IQueryable<T>
    {
        public MyQueryable(Expression expression, IQueryProvider provider)
        {
            Expression = expression;
            Provider = provider;
            ElementType = typeof (T);
        }

        public MyQueryable(IQueryProvider provider)
        {
            Expression = Expression.Constant(this);
            Provider = provider;
            ElementType = typeof(T);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>) Provider.Execute(Expression)).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable) Provider.Execute(Expression)).GetEnumerator();
        }

        public Expression Expression { get; private set; }
        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }

    public class MyQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator
                    .CreateInstance(typeof(MyQueryable<>)
                    .MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new MyQueryable<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            // magic happens here
            // traverse the expression tree and translate it into your target language
            // e.g. SQL, X-Path

            // write out the expression to the console ....
            Console.Out.WriteLine("Converting expression: '{0}'", expression.ToString());

            // but for now just cheat
            // return new List<Customer> { new Customer{ Id = 101, Name = "Mikey boy Hadders" } };
            return 22;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult) Execute(expression);
        }
    }
}