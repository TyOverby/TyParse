using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Test2Doc
{
    class Program
    {
        static string[] TestnameToOutput(string testName, XDocument testResults)
        {
            return testResults.Descendants()
                                 .Where(x => x.Attribute("testName")?.Value?.EndsWith($".{testName}") ?? false)
                                 .Select(x => x.Elements().First().Elements().First().Value)
                                 .First()
                                 .Split("\n");
        }

        static IEnumerable<string> GetTestBodyFor(string input, string[] test)
        {
            int CountLeadSpace(string s) => s.Length - s.TrimStart().TrimStart().Length;

            var startLine = test.TakeWhile(t => !t.Contains($"public void {input}")).Count();
            var openingBraceLine = startLine + 1;
            var openingBraceIndent = CountLeadSpace(test[openingBraceLine]);
            var closingBraceLineOffset = test.Skip(openingBraceLine)
                                             .TakeWhile(t => !(t.Trim().StartsWith("}") && CountLeadSpace(t) == openingBraceIndent))
                                             .Count();
            var lines = test.Skip(openingBraceLine + 1).Take(closingBraceLineOffset - 1);
            foreach (var testline in lines)
            {
                yield return testline.Substring(Math.Min(CountLeadSpace(testline), openingBraceIndent + 4));
            }
        }

        const string PREFIX = "---";
        static IEnumerable<string> Translate(IEnumerable<string> input, string[] testSource, XDocument testResults)
        {
            foreach (var line in input)
            {
                if (line.Trim().StartsWith(PREFIX))
                {
                    var testName = line.Trim().Substring(PREFIX.Length).Trim();
                    yield return "```csharp";
                    foreach (var testLine in GetTestBodyFor(testName, testSource))
                    {
                        if (!testLine.Contains("hide"))
                        {
                            yield return testLine;
                        }
                    }
                    yield return "```";
                    yield return "**output**";
                    yield return "```";
                    foreach (var outputLine in TestnameToOutput(testName, testResults))
                    {
                        yield return outputLine;
                    }
                    yield return "```";
                }
                else
                {
                    yield return line;
                }
            }
        }

        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines("../doc.input.md");
            var testLines = File.ReadAllLines("../TyParse.Tests/Tests.cs");
            var testResults = XDocument.Load("../TyParse.Tests/TestResults/_Tys-MacBook-Pro-2_2018-05-10_21_32_21.trx");

            File.WriteAllLines("../readme.md", Translate(inputLines, testLines, testResults));
        }
    }
}
