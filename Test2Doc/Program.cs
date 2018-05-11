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
            var result = testResults.Descendants()
                                 .Where(x => x.Attribute("testName")?.Value?.EndsWith($".{testName}") ?? false)
                                 .Where(x => x.Elements().Count() != 0)
                                 .Select(x => x.Elements()?.First()?.Elements()?.First()?.Value)
                                 .FirstOrDefault();
            if (result != null)
            {
                return result.Split("\n");
            }
            else
            {
                return Array.Empty<string>();
            }
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

                    var output = TestnameToOutput(testName, testResults);

                    if (output.Length != 0)
                    {
                        yield return "```";
                        foreach (var outputLine in output)
                        {
                            yield return outputLine;
                        }
                        yield return "```";
                    }
                }
                else
                {
                    yield return line;
                }
            }
        }

        static string LatestTrx()
        {
            return Directory.GetFiles("../TyParse.Tests/TestResults").OrderByDescending(a => a).First();
        }

        static void Main(string[] args)
        {
            var inputLines = File.ReadAllLines("../doc.input.md");
            var testLines = File.ReadAllLines("../TyParse.Tests/Tests.cs");
            var testResults = XDocument.Load(LatestTrx());

            File.WriteAllLines("../readme.md", Translate(inputLines, testLines, testResults));
        }
    }
}
