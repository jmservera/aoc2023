using System.Data;

var map = File.ReadAllLines("input.txt");
int x = Array.FindIndex(map, m => m.Contains('S'));
int y = map[x].IndexOf('S');

Console.WriteLine($"Start: {x},{y}");
// 0=up, 1=right, 2=down, 3=left

(int x, int y, int d) nextMove(int x, int y, int direction)
{
    switch (direction)
    {
        case 0:
            if (x > 0 && new char[] { '|', 'F', '7', 'S' }.Contains(map[x - 1][y]))
            {
                switch (map[x - 1][y])
                {
                    case '|':
                        return (x - 1, y, 0);
                    case 'F':
                        return (x - 1, y, 1);
                    case '7':
                        return (x - 1, y, 3);
                    case 'S':
                        return (x - 1, y, -1);
                }
            }
            break;
        case 1:
            if (y < map[x].Length - 1 && new char[] { '-', '7', 'J', 'S' }.Contains(map[x][y + 1]))
            {
                switch (map[x][y + 1])
                {
                    case '-':
                        return (x, y + 1, 1);
                    case '7':
                        return (x, y + 1, 2);
                    case 'J':
                        return (x, y + 1, 0);
                    case 'S':
                        return (x, y + 1, -1);
                }
            }
            break;
        case 2:
            if (x < map.Length - 1 && new char[] { '|', 'J', 'L', 'S' }.Contains(map[x + 1][y]))
            {
                switch (map[x + 1][y])
                {
                    case '|':
                        return (x + 1, y, 2);
                    case 'J':
                        return (x + 1, y, 3);
                    case 'L':
                        return (x + 1, y, 1);
                    case 'S':
                        return (x + 1, y, -1);
                }
            }
            break;
        case 3:
            if (y > 0 && new char[] { '-', 'F', 'L', 'S' }.Contains(map[x][y - 1]))
            {
                switch (map[x][y - 1])
                {
                    case '-':
                        return (x, y - 1, 3);
                    case 'F':
                        return (x, y - 1, 2);
                    case 'L':
                        return (x, y - 1, 0);
                    case 'S':
                        return (x, y - 1, -1);
                }
            }
            break;
    }
    return (-1, -1, -1);

}
int foundDirection = -1;
int lastDirection = -1;

for (int i = 0; i < 4; i++) // 0=up, 1=right, 2=down, 3=left
{
    var (a, b, d) = (x, y, i);
    var length = 0;
    do
    {
        lastDirection = d;
        (a, b, d) = nextMove(a, b, d);
        length++;
    } while ((a, b) != (-1, -1) && map[a][b] != 'S');

    if ((a, b) != (-1, -1) && map[a][b] == 'S')
    {
        foundDirection = i;
        Console.WriteLine($"Found at {a},{b}: {length / 2}");
        Console.WriteLine($"Start {foundDirection} Last: {lastDirection}");
        break;
    }
}

var (ver, hor, dir) = (x, y, foundDirection);

//expand map
var expanded = new char[(map.Length + 1) * 2][];
var s = new string('.', map.Max(l => (l.Length + 1) * 2));
for (int i = 0; i < expanded.Length; i++)
    expanded[i] = s.ToArray();
do
{
    // calc expanded place with margin
    var bx = (ver * 2) + 1;
    var by = (hor * 2) + 1;
    expanded[bx][by] = map[ver][hor];
    switch (dir)
    {
        case 0:
            expanded[bx - 1][by] = '|';
            break;
        case 1:
            expanded[bx][by + 1] = '-';
            break;
        case 2:
            expanded[bx + 1][by] = '|';
            break;
        case 3:
            expanded[bx][by - 1] = '-';
            break;
    }
    (ver, hor, dir) = nextMove(ver, hor, dir);
}
while (map[ver][hor] != 'S');

void fill(int x, int y)
{
    if (expanded[x][y] == '.')
    {
        expanded[x][y] = '0';
        if (x > 0)
            fill(x - 1, y);
        if (x < expanded.Length - 1)
            fill(x + 1, y);
        if (y > 0)
            fill(x, y - 1);
        if (y < expanded[x].Length - 1)
            fill(x, y + 1);
    }
    else
        return;
}

fill(0, 0);

//Console.WriteLine();
//expanded.Select(s => new string(s)).ToList().ForEach(Console.WriteLine);

//draw it
expanded.Chunk(2)
     .Select(a => a[1].Chunk(2).Select(b => b[1]).ToArray())
     .Select(a => new string(a).Replace('0', ' ')
        .Replace('J', '┘').Replace('7', '┐')
        .Replace('L', '└').Replace('F', '┌')
        .Replace('-', '─').Replace('|', '│')
        )
     .ToList().ForEach(Console.WriteLine);


var n = expanded.Chunk(2)
    .Select(a => a[1].Chunk(2).Select(b => b[1]).ToArray())
    .Sum(l => l.Count(c => c == '.'));
Console.WriteLine(n);


