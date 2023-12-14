var lines = File.ReadAllLines("test.txt");

bool show = false;
char[][] tilted = lines.Select(s => s.ToCharArray()).ToArray();

Dictionary<string, (int, int, char[][])> cache = new Dictionary<string, (int, int, char[][])>();

//rotate tilted counter clockwise
char[][] rotate(char[][] rotate)
{
    char[][] rotated = new char[rotate[0].Length][];
    for (int i = 0; i < rotate[0].Length; i++)
    {
        rotated[i] = new char[rotate.Length];
        for (int j = 0; j < rotate.Length; j++)
        {
            rotated[i][j] = rotate[rotate.Length - j - 1][i];
        }
    }
    return rotated;
}

int cacheHits = 0;
int cacheMisses = 0;

for (int cycle = 0; cycle < 10000; cycle++)
{
    if (cycle % 1000000 == 0)
    {
        Console.WriteLine($"{((cycle) * 100.0) / 1000000000}");
    }
    string newTilted = "", oldTilted = "";
    for (int r = 0; r < 4; r++)
    {
        var key = tilted.Select(a => new string(a)).Aggregate((a, b) => a + b);
        if (r == 0)
            newTilted = key;
        if (cache.ContainsKey(key))
        {
            Console.WriteLine($"{cache[key].Item1}, hits:{cache[key].Item2}");
            cacheHits++;
            tilted = cache[key].Item3;
            cache[key] = (cache[key].Item1, cache[key].Item2 + 1, tilted);
            continue;
        }
        else
        {
            cacheMisses++;
        }

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int n = 0; n < lines.Length - 1 - i; n++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (tilted[n][j] == '.' && tilted[n + 1][j] == 'O')
                    {
                        tilted[n][j] = 'O';
                        tilted[n + 1][j] = '.';
                    }
                }
            }
        }
        tilted = rotate(tilted);
        cache[key] = (cache.Count, 0, tilted);
    }
    oldTilted = newTilted;
    newTilted = tilted.Select(a => new string(a)).Aggregate((a, b) => a + b);
    if (oldTilted == newTilted)
    {
        Console.WriteLine($"Cycle: {cycle}");
        Console.WriteLine(newTilted);
        break;
    }
}
Console.WriteLine($"Cache hits: {cacheHits}, cache misses: {cacheMisses}, cache size: {cache.Count}");

tilted.ToList().ForEach(a =>
            Console.WriteLine(new string(a)));

long sum = tilted.Select((a, i) => (a, i)).Sum(a => a.a.Count(b => b == 'O') * (tilted.Length - a.i));
Console.WriteLine(sum);

Console.WriteLine((tilted.Length, tilted[0].Length));

