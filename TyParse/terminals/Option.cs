using System;
using System.Collections.Immutable;

namespace TyParse
{
    public sealed class Option : Parser<string>
    {
        private readonly string longName;
        private readonly bool required;
        private readonly char shortName;
        private readonly string documentation;

        public Option(string longName, bool required = false, char shortName = '\0', string documentation = null)
        {
            if (string.IsNullOrWhiteSpace(longName))
            {
                throw new ArgumentException("message", nameof(longName));
            }

            this.longName = longName;
            this.required = required;
            this.shortName = shortName;
            this.documentation = documentation;
        }

        public Result<string> Parse(ImmutableArray<string> input)
        {
            {
                int longNameIndex = input.IndexOf("--" + longName);
                if (longNameIndex != -1)
                {
                    if (longNameIndex + 1 < input.Length)
                    {
                        var value = input[longNameIndex + 1];
                        return new Ok<string>(value, input.RemoveAt(longNameIndex + 1).RemoveAt(longNameIndex));
                    }
                    else
                    {
                        return new Err<string>($"expected value for option {longName} but found nothing", input);
                    }
                }
            }

            if (shortName != '\0')
            {
                int shortNameIndex = input.IndexOf("-" + shortName);
                if (shortNameIndex != -1)
                {
                    if (shortNameIndex + 1 < input.Length)
                    {
                        var value = input[shortNameIndex + 1];
                        return new Ok<string>(value, input.RemoveAt(shortNameIndex));
                    }
                    else
                    {
                        return new Err<string>($"expected value for option {shortName} but found nothing", input);
                    }
                }
            }

            // TODO: add support for combined short names -xyz

            if (required)
            {
                return new Err<string>($"required parameter --{longName} or -{shortName} not found", input);
            }
            else
            {
                return new Ok<string>(null, input);
            }
        }
    }
}
