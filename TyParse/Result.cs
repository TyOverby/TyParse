using System;
using System.Collections.Immutable;

namespace TyParse
{
    public interface Result<T>
    {
    }

    public static class ResultExtensions
    {
        internal static Result<R> Bind<T, R>(this Result<T> result, Func<T, ImmutableArray<String>, Result<R>> binder)
        {
            switch (result)
            {
                case Ok<T> o:
                    return binder(o.Value, o.Remaining);
                case Err<T> e:
                    return new Err<R>(e.Message, e.Remaining);
                default:
                    throw new Exception("SEALED INTERFACES PLEASE");
            }
        }
        public static Result<R> Map<T, R>(this Result<T> result, Func<T, R> mapper)
        {
            switch (result)
            {
                case Ok<T> o:
                    return new Ok<R>(mapper(o.Value), o.Remaining);
                case Err<T> e:
                    return new Err<R>(e.Message, e.Remaining);
                default:
                    throw new Exception("SEALED INTERFACES PLEASE");
            }
        }
    }

    public sealed class Ok<T> : Result<T>
    {
        public T Value { get; }
        public ImmutableArray<string> Remaining { get; }
        public Ok(T value, ImmutableArray<string> remaining)
        {
            this.Value = value;
            this.Remaining = remaining;
        }
    }

    public sealed class Err<T> : Result<T>
    {
        public string Message { get; }
        public ImmutableArray<string> Remaining { get; }
        public Err(string message, ImmutableArray<string> remaining)
        {
            this.Message = message;
            this.Remaining = remaining;
        }
    }
}
