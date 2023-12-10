using System.Data;

var lines = File.ReadAllLines("input.txt");

static long findNext(IList<int> l)
{
    if (l.All(n => n == 0)) return 0;
    var d = l.Zip(l.Skip(1), (a, b) => b - a);
    return l[^1] + findNext(d.ToList());
}

var s = lines
    .Select(l => l.Split().Select(s => int.Parse(s)))
    .Select(l => findNext(l.ToList()))
    .Sum();

Console.WriteLine(s);

static long findPrevious(IList<int> l)
{
    if (l.All(n => n == 0)) return 0;
    var d = l.Zip(l.Skip(1), (a, b) => b - a);
    return l[0] - findPrevious(d.ToList());
}

var s2 = lines
    .Select(l => l.Split().Select(s => int.Parse(s)))
    .Select(l => findPrevious(l.ToList()))
    .Sum();

Console.WriteLine(s2);
