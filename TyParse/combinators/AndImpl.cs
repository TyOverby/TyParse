using System.Collections.Immutable;

namespace TyParse
{
    internal sealed class AndImpl<A, B> : Parser<(A, B)>
    {
        private readonly Parser<A> a;
        private readonly Parser<B> b;

        public AndImpl(Parser<A> a, Parser<B> b)
        {
            this.a = a ?? throw new System.ArgumentNullException(nameof(a));
            this.b = b ?? throw new System.ArgumentNullException(nameof(b));
        }

        public Result<(A, B)> Parse(ImmutableArray<string> input)
        {
            return this.a.Parse(input)
                         .Bind((ar, ac) => b.Parse(ac)
                         .Bind((br, bc) => new Ok<(A, B)>((ar, br), bc)));
        }
    }
}
