using System;
using System.Collections.Immutable;

namespace TyParse
{
    public sealed class RemainingArgs : Parser<ImmutableArray<string>>
    {
        private readonly string friendlyName;
        private readonly string documentation;

        public RemainingArgs(string friendlyName = null, string documentation = null)
        {
            this.friendlyName = friendlyName;
            this.documentation = documentation;
        }

        public Result<ImmutableArray<string>> Parse(ImmutableArray<string> input)
        {
            return new Ok<ImmutableArray<string>>(input, ImmutableArray<string>.Empty);
        }
    }
}
