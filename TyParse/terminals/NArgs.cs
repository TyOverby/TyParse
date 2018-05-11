using System;
using System.Linq;
using System.Collections.Immutable;

namespace TyParse
{
    public sealed class NArgs : Parser<ImmutableArray<string>>
    {
        private readonly string friendlyName;
        private readonly int n;
        private readonly string documentation;

        public NArgs(string friendlyName, int n, string documentation = null)
        {
            if (string.IsNullOrWhiteSpace(friendlyName))
            {
                throw new ArgumentException("message", nameof(friendlyName));
            }

            this.friendlyName = friendlyName;
            this.n = n;
            this.documentation = documentation;
        }
        public Result<ImmutableArray<string>> Parse(ImmutableArray<string> input)
        {
            if (input.Length >= n)
            {
                var fetched = input.Take(n).ToImmutableArray();
                var rest = input.Skip(n).ToImmutableArray();
                return new Ok<ImmutableArray<string>>(fetched, rest);
            }
            else if (input.Length == 0)
            {
                return new Err<ImmutableArray<string>>($"expected {n} arguments for <{friendlyName}> found none.", input);
            }
            else
            {
                return new Err<ImmutableArray<string>>($"expected {n} arguments for <{friendlyName}> found {input.Length}.", input);
            }
        }
    }
}
