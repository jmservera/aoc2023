int sep = 0;
var mirrors = File.ReadAllLines("input.txt")
                .GroupBy(l => l.Length == 0 ? sep++ : sep, (k, g) => g.ToArray())
                .Select(g => g.Where(l => l.Length > 0).ToArray());

static int FindMirrorAxis(string[] g, int exactDiff = 0)
{
    for (int i = 1; i < g.Length; i++)
    {
        int m = exactDiff - g[i].CountDifferences(g[i - 1]);
        if (m >= 0)
        {
            bool found = true;
            for (int x = 1; i + x < g.Length && i - x > 0; x++)
            {
                m -= g[i + x].CountDifferences(g[i - x - 1]);
                if (m < 0)
                {
                    found = false;
                    break;
                }
            }
            if (found && m == 0)
                return i;
        }
    }

    return 0;
}

int CalcMirror(string[] g, int maxDiff = 0)
{
    int mirrorh = FindMirrorAxis(g, maxDiff);
    if (mirrorh != 0)
        return mirrorh * 100;

    return FindMirrorAxis(g.Transpose(s => new string(s.ToArray()))
                            .ToArray(), maxDiff);
}


int l = mirrors.Sum(s => CalcMirror(s));
Console.WriteLine(l);
int l2 = mirrors.Sum(s => CalcMirror(s, 1));
Console.WriteLine(l2);

public static class IEnumerableExtensions
{
    public static int CountDifferences<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return a.Zip(b).Count(c => (c.First != null && !c.First.Equals(c.Second)) || (c.First == null && c.Second != null));
    }

    public static IEnumerable<IEnumerable<S>> Transpose<S>(this IEnumerable<IEnumerable<S>> g)
    {
        return Enumerable.Range(0, g.First().Count())
            .Select(i => g.Select(s => s.ToArray()[i]));
    }

    public static IEnumerable<T> Transpose<S, T>(this IEnumerable<IEnumerable<S>> g, Func<IEnumerable<S>, T> transform)
    {
        return Enumerable.Range(0, g.First().Count())
            .Select(i => transform(g.Select(s => s.ToArray()[i])));
    }
}