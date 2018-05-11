using System;
using System.Collections.Immutable;

namespace TyParse
{
    public sealed class NoArgsLeft : Parser<object>
    {
        public Result<object> Parse(ImmutableArray<string> input)
        {
            if (input.Length == 0)
            {
                return new Ok<object>(new object(), input);
            }
            else
            {
                return new Err<object>($"expected end of command line, found {String.Join(", ", input)}", input);
            }
        }
    }
}
