using System;
using System.Collections.Immutable;

namespace TyParse
{
    public interface Parser<T>
    {
        Result<T> Parse(ImmutableArray<string> input);
    }

    public class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }
    }

    public static class ParserExtensions
    {
        public static T AssumeGoodParse<T>(this Parser<T> parser, ImmutableArray<string> input)
        {
            var res = parser.Parse(input);
            switch (res)
            {
                case Ok<T> ok:
                    return ok.Value;
                case Err<T> err:
                    throw new ParseException(err.Message);
                default:
                    throw new Exception("NO SUBCLASSING RESULT OK");
            }
        }

        public static bool TryParse<T>(this Parser<T> parser, ImmutableArray<string> input, out T output, out string error)
        {
            output = default;
            error = default;

            var res = parser.Parse(input);
            switch (res)
            {
                case Ok<T> ok:
                    output = ok.Value;
                    return true;
                case Err<T> err:
                    error = err.Message;
                    return false;
                default:
                    throw new Exception("SEALED INTERFACES POR FAVOR");
            }
        }
    }
}
