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

--- TutorialSwitch

The first thing that you might notice is that we store the new Switch in a variable with type `Parser<bool>`.
This is because *everything* in TyParse is a `Parser<T>` and since Switches are either on-or-off, `bool` is a
great choice for `T`.

Now let's make a switch and check to see that it's been turned on:

--- PresentSimpleSwitch

As you can see, the longName is the only required constructor parameter, but we can write this same example
with a short name as well:

--- PresentShortSwitch

And just to show you what happens when the switch is not present:

--- NotPresentSimpleSwitch

## Options

Options are similar to Switches, but expect to be followed by a single value.  Some examples in popular software
include the `-X POST` in `curl -X Post www.google.com`, or `--attach-pid 1234` in `lldb --attach-pid 1234`.

All Option parameters are below.

--- TutorialOption

You'll notice that Options are of type `Parser<string>`.  This is because they parse not only the option name,
but the next argument in the command line.

A very simple parser that looks for something of the form `--machine-name <name>` would be

--- SimpleOption

If an option is not present, the value that will be parsed out of it will be `null`.  If you want, you can require
that the option be set by using the `required` constructor parameter.

## Raw Arguments
Often times, the main program that you are running takes arguments directly!  Think `cat file1.txt file2.txt` or
`less foo.cs`.  These call for raw argument manipulation.

### SingleArg
The simplest of these is `SingleArg` which simply parses out the first argument that it sees.

--- SingleArgTutorial

A `SingleArg` is a `Parser<String>` because the argument that it grabs will stay in the raw String type
as shown below.

--- SingleArg

### RemainingArgs

Similar to SingleArg, RemainingArgs gathers *all* the unparsed arguments.

--- RemainingArgsTutorial

`RemainingArgs` is a `Parser<ImmutableArray<String>>` because it takes 0 or more raw command line
parameters strings from th command line.

--- RemainingArgs
