using System.Buffers.Text;

var lines = File.ReadAllLines("input.txt");

static string calcHash(string s)
{
    var b = s.Chunk(4)
        .Select(n => n.Aggregate((byte)0, (a, b) => (byte)((a << 2) + b switch { 'O' => 1, '.' => 2, '#' => 3, _ => 0 })))
        .ToArray();
    return Convert.ToBase64String(b);
}

static string matrixToString(char[][] tilted)
{
    return tilted.Select(a => new string(a)).Aggregate((a, b) => a + '\n' + b);
}
static char[][] stringToMatrix(string s)
{
    return s.Split('\n').Select(a => a.ToCharArray()).ToArray();
}

static char[][] TiltUp(char[][] tilted)
{
    tilted = tilted.Select(a => a.ToArray()).ToArray();
    for (int i = 0; i < tilted.Length - 1; i++)
    {
        for (int n = 0; n < tilted.Length - 1 - i; n++)
        {
            for (int j = 0; j < tilted[i].Length; j++)
            {
                if (tilted[n][j] == '.' && tilted[n + 1][j] == 'O')
                {
                    tilted[n][j] = 'O';
                    tilted[n + 1][j] = '.';
                }
            }
        }
    }
    return tilted;
}

static char[][] rotate(char[][] rotate)
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


Dictionary<string, char[][]> tiltedCalcs = new Dictionary<string, char[][]>();
int tiltedCalcsHits = 0, tiltedCalcsMisses = 0;

char[][] TiltReflectorNWSE(string s)
{
    if (tiltedCalcs.ContainsKey(s))
    {
        tiltedCalcsHits++;
        return tiltedCalcs[s];
    }
    tiltedCalcsMisses++;
    char[][] reflector = stringToMatrix(s);
    for (int tr = 0; tr < 4; tr++)
    {

        reflector = TiltUp(reflector);
        reflector = rotate(reflector);
    }
    tiltedCalcs[s] = reflector;
    return reflector;
}

Dictionary<string, (string, char[][])> cache_next = new Dictionary<string, (string, char[][])>();
char[][] reflector = lines.Select(s => s.ToCharArray()).ToArray();


int cacheHits = 0;
int cacheMisses = 0;
long cycle_length = 1000000000;//1000000000;

string currentKey = matrixToString(reflector);
var currentKeyHash = calcHash(currentKey);

for (long cycle = 0; cycle < cycle_length; cycle++)
{
    if (cycle_length > 100 && cycle % (cycle_length / 200) == 0)
    {
        Console.WriteLine($"{cycle * 100.0 / cycle_length}%");
        Console.WriteLine($"Cache hits: {cacheHits}, cache misses: {cacheMisses}, next cache size: {cache_next.Count}");
        Console.WriteLine($"Tilted calcs hits: {tiltedCalcsHits}, tilted calcs misses: {tiltedCalcsMisses}, tilted calcs size: {tiltedCalcs.Count}");
    }

    if (cache_next.ContainsKey(currentKeyHash))
    {
        cacheHits++;
        (currentKeyHash, reflector) = cache_next[currentKeyHash];
        continue;
    }
    else
    {
        cacheMisses++;
    }

    var newReflector = TiltReflectorNWSE(currentKey);
    var newKey = matrixToString(newReflector);
    var newKeyHash = calcHash(newKey);
    cache_next[currentKeyHash] = (newKeyHash, newReflector);
    currentKey = newKey;
    currentKeyHash = newKeyHash;
    reflector = newReflector;
}

reflector.ToList().ForEach(a =>
            Console.WriteLine(new string(a)));

long sum = reflector.Select((a, i) => (a, i)).Sum(a => a.a.Count(b => b == 'O') * (reflector.Length - a.i));
Console.WriteLine(sum);

Console.WriteLine((reflector.Length, reflector[0].Length));
