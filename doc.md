hello
world

```c#
Parser<bool> lightsAreOnParser = new Switch("lights");

var args = ImmutableArray.Create("--lights");
bool turnLightsOn = lightsAreOnParser.AssumeGoodParse(args);
Console.WriteLine($"lights should be on: {turnLightsOn}");
turnLightsOn.Should().BeTrue(); /* hide */
```
