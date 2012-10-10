using System;

namespace Mike.Spikes.Monad
{
    public class StateMonad
    {
        public void Spike()
        {
            var state = 
                3.ToState<int, string>().Bind(a =>
                "Hello".ToState<string, string>().Bind(b =>
                DateTime.Now.ToState<DateTime, string>().Bind(c =>
                        string.Format("{0} {1} {2}", a, b, c).ToState<string, string>()
                        )));

            var result = state("The state");

            Console.Out.WriteLine("result.Value = {0}", result.Value);
            Console.Out.WriteLine("result.State = {0}", result.State);

            var state1 =
                from a in 3.ToState<int, string>()
                from b in "Hello".ToState<string, string>()
                from c in DateTime.Now.ToState<DateTime, string>()
                select string.Format("{0} {1} {2}", a, b, c);

            var result1 = state1("Some state");

            Console.Out.WriteLine("result1.Value = {0}", result1.Value);
            Console.Out.WriteLine("result1.State = {0}", result1.State);
        }

        public void Spike2()
        {
            var machine =
                from a in GetInitial()
                from b in AddToCurrentDate(a)
                from state in GetState()
                from x in Do(2 + 2)
                from c in GetDayOfWeek(b)
                select string.Format("{0} {1} {2}", x, b, c);

            var result = machine("Starting. ");

            Console.Out.WriteLine("result.Value = {0}", result.Value);
            Console.Out.WriteLine("result.State = {0}", result.State);
        }

        public State<int, string> GetInitial()
        {
            return state => new Result<string, int>(state + " Got Initial 5.", 5);
        }

        public State<string, string> GetState()
        {
            return state => new Result<string, string>(state, state);
        }

        public State<T, string> Do<T>(T value)
        {
            return state => new Result<string, T>(state, value);
        } 

        public State<DateTime, string> AddToCurrentDate(int value)
        {
            return state => new Result<string, DateTime>(
                state + " Added to Current Date.", 
                DateTime.Now.AddDays(value));
        }

        public State<string, string> GetDayOfWeek(DateTime dateTime)
        {
            return state => new Result<string, string>(
                state + " Got day of week.",
                dateTime.DayOfWeek.ToString());
        } 
    }

    public delegate Result<S, A> State<A, S>(S state); 

    public class Result<S, A>
    {
        public S State { get; private set; }
        public A Value { get; private set; }

        public Result(S state, A value)
        {
            State = state;
            Value = value;
        }
    }

    public static class StateExtensions
    {
        public static State<A,S> ToState<A,S>(this A value)
        {
            return state => new Result<S, A>(state, value);
        }

        public static State<B, S> Bind<A, B, S>(this State<A, S> aState, Func<A, State<B, S>> func)
        {
            return state =>
            {
                var aResult = aState(state);
                return func(aResult.Value)(aResult.State);
            };
        }

        public static State<C, S> SelectMany<A, B, C, S>(this State<A, S> a, Func<A, State<B, S>> func, Func<A, B, C> select)
        {
            return a.Bind(avalue => func(avalue).Bind<B, C, S>(bvalue => select(avalue, bvalue).ToState<C,S>()));
        } 

    }
}