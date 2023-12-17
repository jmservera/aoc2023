var lines = File.ReadAllLines("input.txt");

var beams = new List<(int x, int y, int dir)>();
// 0=right, 1=up, 2=left, 3=down
beams.Add((0, 0, 0));

bool[,] map = new bool[lines.Length, lines[0].Length];

static bool isInMap((int x, int y, int dir) pos, string[] map)
{
    return pos.x >= 0 && pos.x < map[0].Length && pos.y >= 0 && pos.y < map.Length;
}

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
        Console.Write(map[y, x] ? '#' : '.');
        count += map[y, x] ? 1 : 0;
    }
    Console.WriteLine();
}

Console.WriteLine(count);