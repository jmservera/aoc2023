using day4.part1;
using System.Text.RegularExpressions;

var cards = new Cards("input.txt");
Console.WriteLine(cards.FullScore());


Console.WriteLine(
    File.ReadAllLines("input.txt")
        .Select(s => s.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries))
        .Select(n => n[..12].Intersect(n[12..]))
        .Select(n => (1 << n.Count()) / 2)
        .Sum());

//Console.WriteLine(value);
// part 2

Console.WriteLine(cards.CalculateStack());
