int sep = 0;
var mirrors = File.ReadAllLines("input.txt")
                .GroupBy(l => l.Length == 0 ? sep++ : sep, (k, g) => g.ToArray())
                .Select(g => g.Where(l => l.Length > 0).ToArray());



static int FindMirrorAxis(string[] g, int exactDiff = 0)
{
    int mirrorIndex = 0;
    for (int i = 1; i < g.Length; i++)
    {
        var m = exactDiff;
        m -= g[i].CountDifferences(g[i - 1]);
        if (m >= 0)
        {
            mirrorIndex = i;
            bool found = true;
            int x = 1;
            while (i + x < g.Length && i - x > 0)
            {
                m -= g[i + x].CountDifferences(g[i - x - 1]);
                if (m < 0)
                {
                    found = false;
                    break;
                }
                x++;
            }
            if (!found)
            {
                mirrorIndex = 0;
            }
            else
            {
                if (m == 0)
                    break;
                else
                    mirrorIndex = 0;
            }
        }
    }

    return mirrorIndex;
}

int CalcMirror(string[] g, int maxDiff = 0)
{
    int mirrorh = FindMirrorAxis(g, maxDiff);
    int mirrorv = 0;
    if (mirrorh == 0)
    {
        string[] transposed = g.Transpose(s => new string(s.ToArray()))
                                .ToArray();
        mirrorv = FindMirrorAxis(transposed, maxDiff);
    }
    return mirrorh * 100 + mirrorv;
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