# bool TyParse(command line)

TyParse is an experimental commandline parser for .net that takes heavy inspiration from
[Parser Combinators](http://www.cs.nott.ac.uk/~pszgmh/monparsing.pdf) commonly found in functional languages.

The main goal is simple: Build small command line parsers that are easy to test and then combine them into
larger parsers, keeping composability all the way to top.

## Switches

"Switches" are the on/off primitive in a command line.  Some popular examples of switches in software
would be the `-l` in `ls -l` or the `--verbose` in `git remote --verbose`.  Switches don't have any associated
data with them, and they are considered "on" if they are present in the command line, and "off" if they
are not present.

```csharp
Parser<bool> lightsAreOnParser = new Switch("lights");

var args = ImmutableArray.Create("--lights");
bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(args);
Console.WriteLine($"lights should be on: {turnLightsOn}");
```
**output**
```
lights should be on: True
```
