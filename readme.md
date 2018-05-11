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

The full constructor for the Switch is below

```csharp
Parser<bool> mySwitch = new Switch(longName: "my-switch",
                                   shortName: 'm',
                                   documentation: "my switch; used for teaching");
```

The first thing that you might notice is that we store the new Switch in a variable with type `Parser<bool>`.
This is because *everything* in TyParse is a `Parser<T>` and since Switches are either on-or-off, `bool` is a
great choice for `T`.

```csharp
Parser<bool> lightsAreOnParser = new Switch("lights");

bool turnLightsOn = lightsAreOnParser.AssumeGoodParse("--lights");
Console.WriteLine($"lights should be on: {turnLightsOn}");
```
```
lights should be on: True
```

As you can see, the longName is the only required constructor parameter, but we can write this same example
with a short name as well!

```csharp
Parser<bool> lightsAreOnParser = new Switch("lights", 'l');

bool turnLightsOn = lightsAreOnParser.AssumeGoodParse("-l");
Console.WriteLine($"lights should be on: {turnLightsOn}");
```
```
lights should be on: True
```

And just to show you what happens when the switch is not present

```csharp
Parser<bool> lightsAreOnParser = new Switch("lights");

bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(/* no arguments!*/);
Console.WriteLine($"lights should be on: {turnLightsOn}");
```
```
lights should be on: False
```

