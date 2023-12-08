var lines = File.ReadAllLines("input.txt");

var moves = lines[0];
var map = lines[2..].Select(l => new { p = l[..3], left = l[7..10], right = l[12..15] }).ToDictionary(x => x.p, x => (x.left, x.right));

var p = map.Where(m => m.Key[2] == 'A').Select(m => m.Key).ToArray();
int steps = 0;
do
{

    Parallel.For(0, p.Length, (i) =>
    {
        foreach (var c in moves)
        {
            p[i] = c == 'R' ? map[p[i]].right : map[p[i]].left;
        }
    });
    steps += moves.Length;
}
while (!p.All(s => s[2] == 'Z'));

Console.WriteLine(steps);