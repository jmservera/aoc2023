var lines = File.ReadAllLines("input.txt");

var cols = new bool[lines[0].Length];

var expanded = lines.Select(line =>
                            {
                                line.Select((c, i) => (c == '#', i)).ToList().ForEach(a => cols[a.i] |= a.Item1); // mark columns with galaxies
                                return line.Contains('#') ? line.ToCharArray() : Enumerable.Repeat('H', line.Length); // mark rows without galaxies
                            })
                        .ToList()
                        .Select(line => line.Select(
                            (c, i) => cols[i] ? c : 'V')); // mark columns without galaxies


static long Calc(IEnumerable<IEnumerable<char>> map, long n)
{
    long h = 0;

    var coordinates = map.SelectMany(line =>
        {
            h += line.First() == 'H' ? n : 1; // increment horizontal coordinate
            long v = 0;
            return line.Select(c =>
                {
                    v += c == 'V' ? n : 1; // increment vertical coordinate
                    return (h, v, c);
                })
                .Where(t => t.c == '#') // filter out galaxies
                .Select(t => (x: t.h, y: t.v));
        })
        .ToList();

    return coordinates.Join(coordinates, c => true, c => true, (a, b) => (a, b)) // Cartesian product
                        .Where(c => c.a.x != c.b.x || c.a.y != c.b.y) // Remove self
                        .Select(c => new[] { c.a, c.b }
                                                .OrderBy(c => c.x)
                                                .ThenBy(c => c.y)
                                                .Chunk(2)
                                                .Select(c => (a: c[0], b: c[1])).First()) // Switch to ordered pairs
                        .Distinct() // Remove duplicates
                        .Select(c => Math.Abs(c.a.x - c.b.x) + Math.Abs(c.a.y - c.b.y)) // Calculate distance
                        .Sum();
}

Console.WriteLine(Calc(expanded, 2));
Console.WriteLine(Calc(expanded, 1000000));
