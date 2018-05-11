using System;
using System.Collections.Immutable;

namespace TyParse
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new Option("x", required: true).TryParse<int>(int.TryParse, "integer");
            var y = new Option("y", required: true).TryParse<int>(int.TryParse, "integer");
            var booost = new Switch("boost", 'b');
            Parser<(int x, int y, bool booost)> both = Combinators.And(x, y, booost);

            if (both.TryParse(args.ToImmutableArray(), out var result, out var error))
            {
                Console.WriteLine($"x: {result.x}");
                Console.WriteLine($"y: {result.y}");
                Console.WriteLine($"booost: {result.booost}");
            }
            else
            {
                Console.WriteLine($"ERROR {error}");
            }
        }
    }
}
