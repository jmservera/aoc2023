namespace day3.part1;

using System;
using System.Text;

public class MapParts
{
    public int Width { get; }
    public int Height { get; }
    public char[,] Map { get; }

    public MapParts(string filename)
    {
        var lines = File.ReadAllLines(filename);


        Width = lines[0].Length;
        Height = lines.Length;
        Map = new char[Width, Height];

        for (var y = 0; y < Height; y++)
        {
            var line = lines[y];
            for (var x = 0; x < Width; x++)
            {
                Map[x, y] = line[x];
            }
        }
    }

    public char Get(int x, int y)
    {
        return Map[x % Width, y];
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                sb.Append(Map[x, y]);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public int SumSerials()
    {
        int sum = 0;
        int x = 0, y = 0;
        int z = 0;
        do
        {
            var (xx, yy, zz) = findNextNumber(x, y);
            z = zz;
            if (z > 0)
            {

                if (nearbyPart(xx, yy, zz) != (-1, -1))
                {
                    int value = parseNumber(xx, yy, zz);
                    sum += value;
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
        return sum;
    }

    protected int parseNumber(int xx, int yy, int zz)
    {
        int value = 0;
        for (int i = 0; i < zz; i++)
        {
            value *= 10;
            value += Map[xx + i, yy] - '0';
        }
        return value;
    }

    protected (int x, int y) nearbyPart(int xx, int yy, int zz)
    {
        int left = xx - 1, right = xx + zz;
        int top = yy - 1, bottom = yy + 1;
        for (int i = left; i <= right; i++)
        {
            if (i < 0 || i >= Width)
            {
                continue;
            }
            for (int j = top; j <= bottom; j++)
            {
                if (j < 0 || j >= Height)
                {
                    continue;
                }
                if (Map[i, j] != '.' && !char.IsDigit(Map[i, j]))
                {
                    return (i, j);
                }
            }
        }
        return (-1, -1);
    }

    protected (int xx, int yy) calcNext(int x, int y, int length)
    {
        x += length;
        if (x >= Width)
        {
            x = 0;
            y++;
        }
        if (y >= Height)
        {
            return (-1, -1);
        }
        return (x, y);
    }

    protected (int x, int y, int length) findNextNumber(int x, int y)
    {
        int xx = x, yy = y;
        int length = 0;
        while (!char.IsDigit(Map[xx, yy]))
        {
            xx++;
            if (xx >= Width)
            {
                xx = 0;
                yy++;
            }
            if (yy >= Height)
            {
                return (xx, yy, 0);
            }
        }
        if (char.IsDigit(Map[xx, yy]))
        {
            while (char.IsDigit(Map[xx + length, yy]))
            {
                length++;
                if (xx + length >= Width)
                {
                    break;
                }
            }
        }
        return (xx, yy, length);
    }
}
