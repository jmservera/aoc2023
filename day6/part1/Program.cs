string[] lines = System.IO.File.ReadAllLines(@"input.txt");

var result = lines[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1..].Select(x => int.Parse(x))
    .Zip(lines[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1..].Select(x => int.Parse(x)),
    (a, b) => ((long)a, (long)b))
    .Calc();

Console.WriteLine(result);

var result2 = lines.Select(l => long.Parse(string.Concat(l.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1..])))
    .Chunk(2).ToList().Select(x => (x[0], x[1])).Calc();
Console.WriteLine(result2);

public static class Extensions
{
    public static double Calc(this IEnumerable<(long a, long b)> x) =>
    x.Select(x => ((x.a + Math.Sqrt((x.a * x.a) - 4 * x.b)) / 2, (x.a - Math.Sqrt((x.a * x.a) - 4 * x.b)) / 2))
    .Select(x => (x.Item1 > x.Item2) ? (x.Item1, x.Item2) : (x.Item2, x.Item1))
    .Select(x => (Math.Ceiling(x.Item1) - 1, Math.Floor(x.Item2) + 1))
    .Select(x => x.Item1 - x.Item2 + 1)
    .Aggregate((a, b) => a * b);
}

