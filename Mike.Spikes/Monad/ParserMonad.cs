using System;
using System.Collections.Generic;
using System.Linq;

namespace Mike.Spikes.Monad
{
    public class ParserMonad
    {
        public void Spike()
        {
            var parser =
                from a in Parse.Char(x => x == 'a')
                from b in Parse.Char(x => x == 'b')
                select a.ToString() + b.ToString();

            var result = parser.Parse("ab");
            Console.Out.WriteLine("result = {0}", result);

            var manyParser =
                from a in Parse.Char(x => x == 'a').Many()
                from b in Parse.Char(x => x == 'b').Many()
                select new string(a.Concat(b).ToArray());

            var manyResult = manyParser.Parse("aaaabbbbb_");
            Console.Out.WriteLine("manyResult = {0}", manyResult);
        }
    }

    public delegate IResult<T> Parser<T>(Input input);

    public interface IResult<T>
    {
    }

    public class Failure<T> : IResult<T>
    {
        public string Message { get; private set; }

        public Failure(string message)
        {
            Message = message;
        }
    }

    public class Success<T> : IResult<T>
    {
        public T Value { get; private set; }
        public Input Remainder { get; private set; }

        public Success(T value, Input remainder)
        {
            Value = value;
            Remainder = remainder;
        }
    }

    public class Input
    {
        private readonly IEnumerator<char> enumerator;
        private bool atEndOfStream;

        public Input(IEnumerator<char> value)
        {
            enumerator = value;
            MoveNext();
        }

        public void MoveNext()
        {
            atEndOfStream = !enumerator.MoveNext();
        }

        public char GetCurrentChar()
        {
            if (atEndOfStream)
            {
                throw new ApplicationException("End of input stream");
            }

            return enumerator.Current;
        }
    }

    public static class ParserExtensions
    {
        public static Input ToInput(this string input)
        {
            return new Input(input.GetEnumerator());
        }

        public static Parser<T> ToParser<T>(this T value)
        {
            return input => new Success<T>(value, "".ToInput());
        }

        public static T Parse<T>(this Parser<T> parser, string input)
        {
            var result = parser(input.ToInput());
            var fail = result as Failure<T>;
            if (fail != null)
            {
                throw new ApplicationException(fail.Message);
            }
            var success = result as Success<T>;
            if (success != null)
            {
                return success.Value;
            }
            throw new ApplicationException("Result was neither failure or success");
        }

        public static Parser<B> Then<A, B>(this Parser<A> a, Func<A, Parser<B>> func)
        {
            return input =>
            {
                var aResult = a(input);
                var aFail = aResult as Failure<A>;
                if (aFail != null)
                {
                    return new Failure<B>(aFail.Message);
                }
                var aSuccess = aResult as Success<A>;
                if (aSuccess != null)
                {
                    var parserB = func(aSuccess.Value);
                    return parserB(aSuccess.Remainder);
                }
                throw new ApplicationException("Result was neither success or failure");
            };
        }

        public static Parser<C> SelectMany<A, B, C>(this Parser<A> a, Func<A, Parser<B>> func,
            Func<A, B, C> select)
        {
            return a.Then(avalue => func(avalue).Then<B, C>(bvalue => select(avalue, bvalue).ToParser()));
        }
 
        public static Parser<IEnumerable<T>> Many<T>(this Parser<T> parser)
        {
            return input => new Success<IEnumerable<T>>(IterateParserOverInput(parser, input), input);
        }

        private static IEnumerable<T> IterateParserOverInput<T>(Parser<T> parser, Input input)
        {
            while (true)
            {
                var result = parser(input);
                var success = result as Success<T>;
                if (success != null)
                {
                    yield return success.Value;
                }
                else
                {
                    break;
                }
            }
        }
    }

    public static class Parse
    {
        public static Parser<char> Char(Func<char, bool> predicate)
        {
            return input =>
            {
                var nextChar = input.GetCurrentChar();
                if (predicate(nextChar))
                {
                    input.MoveNext();
                    return new Success<char>(nextChar, input);
                }
                return new Failure<char>(string.Format("Unexpected char '{0}'", nextChar));
            };
        }
    }
}