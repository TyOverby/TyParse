using System;
using System.Collections.Immutable;

namespace TyParse
{
    public sealed class SingleArg : Parser<string>
    {
        private readonly string friendlyName;
        private readonly string documentation;

        public SingleArg(string friendlyName = null, string documentation = null)
        {
            this.friendlyName = friendlyName;
            this.documentation = documentation;
        }
        public Result<string> Parse(ImmutableArray<string> input)
        {
            if (input.Length == 1)
            {
                return new Ok<string>(input[0], input.RemoveAt(0));
            }
            else if (input.Length == 0)
            {
                return new Err<string>($"expected single argument for <{friendlyName}> found multiple: {String.Join(", ", input)}", input);
            }
            else
            {
                return new Err<string>($"expected single argument for <{friendlyName}> found multiple: {String.Join(", ", input)}", input);
            }
        }
    }
}
