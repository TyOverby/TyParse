using System;
using System.Collections.Immutable;

namespace TyParse
{
    internal sealed class BindImpl<T, R> : Parser<R>
    {
        private readonly Parser<T> parser;
        private readonly Func<T, Func<R, Result<R>>, Func<string, Result<R>>, Result<R>> binder;

        public BindImpl(Parser<T> parser, Func<T, Func<R, Result<R>>, Func<string, Result<R>>, Result<R>> binder)
        {
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.binder = binder ?? throw new ArgumentNullException(nameof(binder));
        }

        public Result<R> Parse(ImmutableArray<string> input) =>
            this.parser.Parse(input).Bind((t, arr) => binder(t, (r) => new Ok<R>(r, arr), (e) => new Err<R>(e, arr)));
    }
}
