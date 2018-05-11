using System;
using System.Collections.Immutable;
using Xunit;
using TyParse;
using Xunit.Abstractions;
using FluentAssertions;

namespace TyParse.Tests
{
    public class Tests
    {
        private readonly ITestOutputHelper Console;

        public Tests(ITestOutputHelper output)
        {
            this.Console = output ?? throw new ArgumentNullException(nameof(output));
        }

        [Fact]
        public void PresentSimpleSwitch()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights");

            var args = ImmutableArray.Create("--lights");
            bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(args);
            Console.WriteLine($"lights should be on: {turnLightsOn}");
            turnLightsOn.Should().BeTrue(); /* hide */
        }

        [Fact]
        public void PresentShortSwitch()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights", 'l');

            var args = ImmutableArray.Create("-l");
            bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(args);
            Console.WriteLine($"lights should be on: {turnLightsOn}");
            turnLightsOn.Should().BeTrue(); /* hide */
        }

        [Fact]
        public void NotPresentSimpleSwitch()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights");

            var args = ImmutableArray<string>.Empty;
            bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(args);
            Console.WriteLine($"lights should be on: {turnLightsOn}");
            turnLightsOn.Should().BeFalse(); /* hide */
        }

        [Fact]
        public void AndCombinator()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights");
            Parser<bool> fanIsOnParser = new Switch("fan");
            Parser<(bool lights, bool fan)> lightsAndFanParser = Combinators.And(lightsAreOnParser, fanIsOnParser);

            // command line: --lights --fan
            var args = ImmutableArray.Create("--lights", "--fan");
            var both = lightsAndFanParser.AssumeGoodParse(args);
            Console.WriteLine($"lights : {both.lights} | fan : {both.fan}");
            both.lights.Should().BeTrue(); /* hide */
            both.fan.Should().BeTrue(); /* hide */

            // command line: --lights
            args = ImmutableArray.Create("--lights");
            both = lightsAndFanParser.AssumeGoodParse(args);
            Console.WriteLine($"lights : {both.lights} | fan : {both.fan}");
            both.lights.Should().BeTrue(); /* hide */
            both.fan.Should().BeFalse(); /* hide */

            // command line: <empty>
            args = ImmutableArray<string>.Empty;
            both = lightsAndFanParser.AssumeGoodParse(args);
            Console.WriteLine($"lights : {both.lights} | fan : {both.fan}");
            both.lights.Should().BeFalse(); /* hide */
            both.fan.Should().BeFalse(); /* hide */
        }

        [Fact]
        public void SimpleOption()
        {
            Parser<string> computerNameParser = new Option("machine");

            // command line: --machine tyoverby-linux-001
            var args = ImmutableArray.Create("--machine", "tyoverby-linux-001");
            var computer = computerNameParser.AssumeGoodParse(args);
            Console.WriteLine($"selected machine: {computer}");
            computer.Should().Be("tyoverby-linux-001"); /* hide */

            // command line: <empty>
            args = ImmutableArray<string>.Empty;
            computer = computerNameParser.AssumeGoodParse(args);
            Console.WriteLine($"selected machine: {computer ?? "<null>"}");
            computer.Should().BeNull(); /* hide */
        }

        [Fact]
        public void SingleArg()
        {
            Parser<string> singleArgumentParser = new SingleArg();

            // command line: foo.txt
            var args = ImmutableArray.Create("foo.txt");
            var argument = singleArgumentParser.AssumeGoodParse(args);
            Console.WriteLine($"argument is: {argument}");
            argument.Should().Be("foo.txt"); /* hide */
        }

        [Fact]
        public void RemainingArgs()
        {
            Parser<ImmutableArray<string>> singleArgumentParser = new RemainingArgs();

            // command line: foo.txt bar.txt
            var args = ImmutableArray.Create("foo.txt", "bar.txt");
            var arguments = singleArgumentParser.AssumeGoodParse(args);
            Console.WriteLine($"argument is: {String.Join(",", arguments)}");
            arguments.Should().BeEquivalentTo(args); /* hide */
        }
    }
}
