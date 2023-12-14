var lines = File.ReadAllLines("test.txt");

char[][] tilted = lines.Select(s => s.ToCharArray()).ToArray();

// Dictionary<string, char[][]> cache = new Dictionary<string, char[][]>();
Dictionary<string, (string, char[][])> cache_next = new Dictionary<string, (string, char[][])>();

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

static string tiltedToString(char[][] tilted)
{
    return tilted.Select(a => new string(a)).Aggregate((a, b) => a + b);
}

int cacheHits = 0;
int cacheMisses = 0;
int cacheNextHits = 0;
int cacheNextMisses = 0;

long cycle_length = 3000;//1000000000;

string nextKeyCached = "";
string lastKey = "";

for (long cycle = 0; cycle < cycle_length; cycle++)
{
    if (cycle_length > 100 && cycle % (cycle_length / 100) == 0)
    {
        Console.WriteLine($"{cycle * 100.0 / cycle_length}%");
        Console.WriteLine($"Cache hits: {cacheHits}, cache misses: {cacheMisses},  next cache hits: {cacheNextHits}, next cache misses: {cacheNextMisses} next cache size: {cache_next.Count}");
    }
    var currentTiltKey = nextKeyCached == "" ? tiltedToString(tilted) : nextKeyCached;

    if (cache_next.ContainsKey(currentTiltKey))
    {
        cacheNextHits++;
        nextKeyCached = cache_next[currentTiltKey].Item1;
        tilted = cache_next[currentTiltKey].Item2;
        lastKey = currentTiltKey;
        continue;
    }
    else
    {
        cacheNextMisses++;
    }
    nextKeyCached = "";
    for (int tr = 0; tr < 4; tr++)
    {
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
    }

    if (lastKey != "")
    {
        if (cache_next.ContainsKey(lastKey))
        {
            if (cache_next[lastKey].Item1 != currentTiltKey)
            {
                Console.WriteLine("last key not equal");
            }
            else
            {
                Console.WriteLine("last key equal");
            }
        }
        cache_next[lastKey] = (tiltedToString(tilted), tilted);
        Console.WriteLine($"last key: {lastKey}\nnext: {cache_next[lastKey].Item1}");
        tilted.ToList().ForEach(a =>
            Console.WriteLine(new string(a)));
    }
    else
    {
        Console.WriteLine("no last key");
    }
    lastKey = tiltedToString(tilted);
}

tilted.ToList().ForEach(a =>
            Console.WriteLine(new string(a)));

long sum = tilted.Select((a, i) => (a, i)).Sum(a => a.a.Count(b => b == 'O') * (tilted.Length - a.i));
Console.WriteLine(sum);

Console.WriteLine((tilted.Length, tilted[0].Length));

cache_next[lastKey].Item2.ToList().ForEach(a =>
            Console.WriteLine(new string(a)));

// foreach (var key in cache_next.Keys)
// {
//     Console.WriteLine($"Key:  {key}\nnext: {cache_next[key].Item1}");
//     cache_next[key].Item2.ToList().ForEach(a =>
//             Console.WriteLine(new string(a)));
//     Console.WriteLine();
// }

