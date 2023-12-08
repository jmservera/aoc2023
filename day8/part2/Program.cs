var lines = File.ReadAllLines("input.txt");

static long gcf(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static long lcm(long a, long b)
{
    return (a / gcf(a, b)) * b;
}

var moves = lines[0];
var map = lines[2..].Select(l => new { p = l[..3], left = l[7..10], right = l[12..15] }).ToDictionary(x => x.p, x => (x.left, x.right));

var p = map.Where(m => m.Key[2] == 'A').Select(m => m.Key).ToArray();
long[] steps = new long[p.Length];
for (int i = 0; i < p.Length; i++)
{
    do
    {
        foreach (var c in moves)
        {
            p[i] = c == 'R' ? map[p[i]].right : map[p[i]].left;
        }
        steps[i] += moves.Length;
    }
    while (p[i][2] != 'Z');
}

steps.ToList().ForEach(c => Console.WriteLine(c));


Console.WriteLine(steps.Aggregate(lcm));