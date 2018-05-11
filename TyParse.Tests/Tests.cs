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
        public void TutorialSwitch()
        {
            Parser<bool> mySwitch = new Switch(longName: "my-switch",
                                               shortName: 'm',
                                               documentation: "my switch; used for teaching");
        }

        [Fact]
        public void PresentSimpleSwitch()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights");

            bool turnLightsOn = lightsAreOnParser.AssumeGoodParse("--lights");
            Console.WriteLine($"lights should be on: {turnLightsOn}");
            turnLightsOn.Should().BeTrue(); /* hide */
        }

        [Fact]
        public void PresentShortSwitch()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights", 'l');

            bool turnLightsOn = lightsAreOnParser.AssumeGoodParse("-l");
            Console.WriteLine($"lights should be on: {turnLightsOn}");
            turnLightsOn.Should().BeTrue(); /* hide */
        }

        [Fact]
        public void NotPresentSimpleSwitch()
        {
            Parser<bool> lightsAreOnParser = new Switch("lights");

            bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(/* no arguments!*/);
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
            var both = lightsAndFanParser.AssumeGoodParse("--lights", "--fan");
            Console.WriteLine($"lights : {both.lights} | fan : {both.fan}");
            both.lights.Should().BeTrue(); /* hide */
            both.fan.Should().BeTrue(); /* hide */

            // command line: --lights
            both = lightsAndFanParser.AssumeGoodParse("--lights");
            Console.WriteLine($"lights : {both.lights} | fan : {both.fan}");
            both.lights.Should().BeTrue(); /* hide */
            both.fan.Should().BeFalse(); /* hide */

            // command line: <empty>
            both = lightsAndFanParser.AssumeGoodParse(/* no args! */);
            Console.WriteLine($"lights : {both.lights} | fan : {both.fan}");
            both.lights.Should().BeFalse(); /* hide */
            both.fan.Should().BeFalse(); /* hide */
        }

        [Fact]
        public void TutorialOption()
        {
            Parser<string> myOption = new Option(longName: "my-option",
                                               required: false,
                                               shortName: 'm',
                                               documentation: "my option type, used for teaching");
        }

        [Fact]
        public void SimpleOption()
        {
            Parser<string> computerNameParser = new Option("machine");

            // command line: --machine tyoverby-linux-001
            var computer = computerNameParser.AssumeGoodParse("--machine", "tyoverby-linux-001");
            Console.WriteLine($"selected machine: {computer}");
            computer.Should().Be("tyoverby-linux-001"); /* hide */

            // command line: <empty>
            computer = computerNameParser.AssumeGoodParse(/* no args! */);
            Console.WriteLine($"selected machine: {computer ?? "<null>"}");
            computer.Should().BeNull(); /* hide */
        }

        [Fact]
        public void SingleArgTutorial()
        {
            Parser<string> singleArgumentParser = new SingleArg(
                friendlyName: "my-one-arg",
                documentation: "a single argument will be parsed");
        }

        [Fact]
        public void SingleArg()
        {
            Parser<string> singleArgumentParser = new SingleArg();

            // command line: foo.txt
            var argument = singleArgumentParser.AssumeGoodParse("foo.txt");
            Console.WriteLine($"argument is: {argument}");
            argument.Should().Be("foo.txt"); /* hide */
        }

        [Fact]
        public void RemainingArgsTutorial()
        {
            Parser<ImmutableArray<string>> singleArgumentParser =
                new RemainingArgs(
                    friendlyName: "my-remaining-args",
                    documentation: "docs go here");
        }
        [Fact]
        public void RemainingArgs()
        {
            Parser<ImmutableArray<string>> singleArgumentParser = new RemainingArgs();

            // command line: foo.txt bar.txt
            var arguments = singleArgumentParser.AssumeGoodParse("foo.txt", "bar.txt");
            Console.WriteLine($"argument is: {String.Join(",", arguments)}");
            arguments.Should().BeEquivalentTo(ImmutableArray.Create("foo.txt", "bar.txt")); /* hide */
        }
    }
}
