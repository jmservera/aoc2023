var lines = File.ReadAllLines("test.txt");

var expanded = new List<List<char>>();
var cols = new bool[lines[0].Length];
foreach (var line in lines)
{
    if (!line.Contains('#'))
    {
        expanded.Add(line.ToCharArray().ToList());
    }
    expanded.Add(line.ToCharArray().ToList());

    line.Select((c, i) => (c == '#', i)).ToList().ForEach(a => cols[a.i] |= a.Item1);
}

cols.Select((b, i) => (b, i)).Reverse().ToList().ForEach(c =>
{
    if (!c.b)
    {
        expanded.ForEach(r => r.Insert(c.i, '.'));
    }
});

expanded.ForEach(r => Console.WriteLine(new string(r.ToArray())));

var coordinates = expanded.Select((l, x) => (l, x))
    .Where(line => line.l.Contains('#'))
    .Select(line => line.l.Select((c, y) => (c, y)).Where(l => l.c == '#').Select(c => (line.x, c.y)))
    .SelectMany(c => c).Select(c => (c.x, c.y)).ToList();


var sum = coordinates.Join(coordinates, c => true, c => true, (a, b) => (a, b))
   .Where(c => c.a.x != c.b.x || c.a.y != c.b.y)
   .Select(c => new[] { c.a, c.b }
                        .OrderBy(c => c.x)
                        .ThenBy(c => c.y)
                        .Chunk(2)
                        .Select(c => (a: c[0], b: c[1])).First())
   .Distinct()
   .Select(c => Math.Abs(c.a.x - c.b.x) + Math.Abs(c.a.y - c.b.y))
   .Sum();

Console.WriteLine(sum);

