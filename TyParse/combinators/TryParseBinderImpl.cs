using System;
using System.Collections.Immutable;

namespace TyParse
{
    internal class TryParseBinderImpl<T> : Parser<T>
    {
        private readonly Parser<string> parser;
        private readonly TryParseDelegate<T> delagate;
        private readonly string typeMessage;

        public TryParseBinderImpl(Parser<String> parser, TryParseDelegate<T> delagate, string typeMessage)
        {
            if (string.IsNullOrWhiteSpace(typeMessage))
            {
                throw new ArgumentException("message", nameof(typeMessage));
            }

            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.delagate = delagate ?? throw new ArgumentNullException(nameof(delagate));
            this.typeMessage = typeMessage;
        }

        public Result<T> Parse(ImmutableArray<string> input)
        {
            return parser.Bind<string, T>((s, g, b) =>
            {
                if (delagate(s, out var res))
                {
                    return g(res);
                }
                else
                {
                    return b($"expected {s} to be a {typeMessage}");
                }
            }).Parse(input);
        }
    }
}
