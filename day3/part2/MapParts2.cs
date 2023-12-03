namespace day3.part2;

using System;
using System.Security.Cryptography;
using day3.part1;

public class MapParts2 : MapParts
{
    public MapParts2(string filename) : base(filename)
    {
    }

    public int MultiplyGears()
    {
        Dictionary<(int, int), List<int>> parts = new Dictionary<(int, int), List<int>>();

        int x = 0, y = 0;
        int z = 0;
        do
        {
            var (xx, yy, zz) = findNextNumber(x, y);
            z = zz;
            if (z > 0)
            {
                var (nx, ny) = nearbyPart(xx, yy, zz);
                if (nx != -1)
                {
                    if (Get(nx, ny) == '*')
                    {
                        if (!parts.ContainsKey((nx, ny)))
                        {
                            parts.Add((nx, ny), new List<int>());
                        }
                        parts[(nx, ny)].Add(parseNumber(xx, yy, zz));
                    }
                }

                (xx, yy) = calcNext(xx, yy, zz);
                if (xx >= 0)
                {
                    x = xx;
                    y = yy;
                }
                else
                {
                    break;
                }
            }
        } while (z > 0);

        var s = parts.Where(p => p.Value.Count == 2).Sum(t => t.Value[0] * t.Value[1]);
        return s;
    }

}