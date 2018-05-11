using System;
using System.Collections.Immutable;

namespace TyParse
{
    public class Switch : Parser<bool>
    {
        private readonly string longName;
        private readonly char shortName;
        private readonly string documentation;

        public Switch(string longName, char shortName = '\0', string documentation = null)
        {
            if (shortName == '-' || longName.StartsWith("-"))
            {
                throw new ArgumentException("short name and long name must not start with '-'");
            }

            this.longName = longName;
            this.shortName = shortName;
            this.documentation = documentation;
        }

        public Result<bool> Parse(ImmutableArray<string> input)
        {
            {
                int longNameIndex = input.IndexOf("--" + longName);
                if (longNameIndex != -1)
                {
                    return new Ok<bool>(true, input.RemoveAt(longNameIndex));
                }
            }

            if (shortName != '\0')
            {
                int shortNameIndex = input.IndexOf("-" + shortName);
                if (shortNameIndex != -1)
                {
                    return new Ok<bool>(true, input.RemoveAt(shortNameIndex));
                }
            }

            // TODO: add support for combined short names -xyz

            return new Ok<bool>(false, input);
        }
    }
}
