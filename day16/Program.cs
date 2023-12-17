using System.Runtime.InteropServices;

var lines = File.ReadAllLines("input.txt");

static bool isInMap((int x, int y, int dir) pos, string[] map)
{
    return pos.x >= 0 && pos.x < map[0].Length && pos.y >= 0 && pos.y < map.Length;
}

static int calculateBeamStrength((int x, int y, int dir) startingBeam, string[] lines)
{
    bool[,] map = new bool[lines.Length, lines[0].Length];
    var beams = new List<(int x, int y, int dir)>();
    // 0=right, 1=up, 2=left, 3=down
    beams.Add(startingBeam);

    do
    {
        var newBeams = new List<(int x, int y, int dir)>();
        foreach (var beam in beams)
        {
            if (lines[beam.y][beam.x] != '.')
            {
                switch (lines[beam.y][beam.x])
                {
                    case '-':
                        {
                            if (map[beam.y, beam.x])
                                break;
                            var newBeam = (beam.x + 1, beam.y, 0);
                            if (isInMap(newBeam, lines))
                                newBeams.Add(newBeam);
                            newBeam = (beam.x - 1, beam.y, 2);
                            if (isInMap(newBeam, lines))
                                newBeams.Add(newBeam);
                        }
                        break;
                    case '|':
                        {
                            if (map[beam.y, beam.x])
                                break;
                            var newBeam = (beam.x, beam.y - 1, 1);
                            if (isInMap(newBeam, lines))
                                newBeams.Add(newBeam);
                            newBeam = (beam.x, beam.y + 1, 3);
                            if (isInMap(newBeam, lines))
                                newBeams.Add(newBeam);
                        }
                        break;
                    case '/':
                        {
                            switch (beam.dir)
                            {
                                case 0: // right > up
                                    {
                                        var newBeam = (beam.x, beam.y - 1, 1);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;
                                case 1: // up > right
                                    {
                                        var newBeam = (beam.x + 1, beam.y, 0);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;
                                case 2: //left > down
                                    {
                                        var newBeam = (beam.x, beam.y + 1, 3);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;
                                case 3: //down > left
                                    {
                                        var newBeam = (beam.x - 1, beam.y, 2);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;

                            }
                        }
                        break;
                    case '\\':
                        {
                            switch (beam.dir)
                            {
                                case 0: // right > down
                                    {
                                        var newBeam = (beam.x, beam.y + 1, 3);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;
                                case 1: // up > left
                                    {
                                        var newBeam = (beam.x - 1, beam.y, 2);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;
                                case 2: //left > up
                                    {
                                        var newBeam = (beam.x, beam.y - 1, 1);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;
                                case 3: //down > right
                                    {
                                        var newBeam = (beam.x + 1, beam.y, 0);
                                        if (isInMap(newBeam, lines))
                                            newBeams.Add(newBeam);
                                    }
                                    break;

                            }
                        }
                        break;
                }

            }
            else
            {
                var newBeam = (beam.dir == 0 ? beam.x + 1 : beam.dir == 2 ? beam.x - 1 : beam.x,
                beam.dir == 1 ? beam.y - 1 : beam.dir == 3 ? beam.y + 1 : beam.y,
                beam.dir);
                if (isInMap(newBeam, lines))
                    newBeams.Add(newBeam);
            }
            map[beam.y, beam.x] = true;
        }
        beams = newBeams;

    } while (beams.Count > 0);

    int count = 0;
    for (int y = 0; y < map.GetLength(0); y++)
    {
        for (int x = 0; x < map.GetLength(1); x++)
        {
            // Console.Write(map[y, x] ? '#' : '.');
            count += map[y, x] ? 1 : 0;
        }
        // Console.WriteLine();
    }
    return count;
}

// part 1
Console.WriteLine(calculateBeamStrength((0, 0, 0), lines));

// part 2
// 0=right, 1=up, 2=left, 3=down

var beamsToTest = new List<(int x, int y, int dir)>();
for (int y = 0; y < lines.Length; y++)
{
    beamsToTest.Add((0, y, 0));
    beamsToTest.Add((lines[0].Length - 1, y, 2));
    if (y != 0)
    {
        beamsToTest.Add((0, y, 1));
        beamsToTest.Add((lines[0].Length - 1, y, 1));
    }
    if (y != lines.Length - 1)
    {
        beamsToTest.Add((0, y, 3));
        beamsToTest.Add((lines[0].Length - 1, y, 3));
    }
}
for (int x = 1; x < lines[0].Length - 1; x++)
{
    beamsToTest.Add((x, 0, 3));
    beamsToTest.Add((x, lines.Length - 1, 1));
    beamsToTest.Add((x, 0, 0));
    beamsToTest.Add((x, lines.Length - 1, 2));
}
var count = 0;
foreach (var beam in beamsToTest)
{
    var newCount = calculateBeamStrength(beam, lines);
    count = Math.Max(count, newCount);
}

Console.WriteLine(count);