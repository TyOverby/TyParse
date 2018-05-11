using System;
using System.Collections.Immutable;

namespace TyParse
{

    public sealed class Command : Parser<string>
    {
        private readonly string command;
        private readonly string documentation;

        public Command(string command, string documentation = null)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("message should not be null or whitespace", nameof(command));
            }

            this.command = command;
            this.documentation = documentation;
        }


        public Result<string> Parse(ImmutableArray<string> input)
        {
            if (input.Length == 0)
            {
                return new Err<string>($"command `{command}` not found, got empty input", input);
            }
            else if (input.Length != 0 && input[0] == command)
            {
                return new Ok<string>(command, input.RemoveAt(0));
            }
            else
            {
                return new Err<string>($"command `{command}` not found, found {input[0]}", input);
            }
        }
    }
}
