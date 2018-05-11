using System;
using System.Collections.Immutable;

namespace TyParse
{
    internal sealed class MapImpl<T, R> : Parser<R>
    {
        private readonly Parser<T> parser;
        private readonly Func<T, R> mapper;

        internal MapImpl(Parser<T> parser, Func<T, R> mapper)
        {
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Result<R> Parse(ImmutableArray<string> input)
        {
            return this.parser.Parse(input).Map(mapper);
        }
    }
}
