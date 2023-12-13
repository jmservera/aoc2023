

using System.Security.Cryptography.X509Certificates;

var lines = File.ReadAllLines("input.txt");
int sep = 0;
int counter = 0;



bool areColsEqual(string[] g, int col1, int col2)
{
    for (int i = 0; i < g.Length; i++)
    {
        if (g[i][col1] != g[i][col2])
        {
            return false;
        }
    }
    return true;
}
int Mirror(string[] g)
{
    int mirrorh = 0, mirrorv = 0;
    for (int i = 1; i < g.Length; i++)
    {
        if (g[i] == g[i - 1])
        {
            mirrorh = i;
            bool found = true;
            int x = 1;
            while (i + x < g.Length && i - x > 0)
            {
                if (g[i + x] != g[i - x - 1])
                {
                    found = false;
                    break;
                }
                x++;
            }
            if (!found)
            {
                mirrorh = 0;
            }
            else
            {
                break;

            }
        }
    }
    if (mirrorh == 0)
    {
        for (int i = 1; i < g[0].Length; i++)
        {
            if (areColsEqual(g, i, i - 1))
            {
                mirrorv = i;
                bool found = true;
                int x = 1;
                while (i + x < g[0].Length && i - x > 0)
                {
                    if (!areColsEqual(g, i + x, i - x - 1))
                    {
                        found = false;
                        break;
                    }
                    x++;
                }
                if (!found)
                {
                    mirrorv = 0;
                }
                else
                {
                    break;
                }

            }
        }
    }
    // if (mirrorh == 0 && mirrorv == 0)
    // {
    //     Console.ForegroundColor = ConsoleColor.Red;
    //     for (int i = 0; i < g.Length; i++)
    //     {
    //         Console.WriteLine(g[i]);
    //     }
    // }
    // Console.WriteLine($"{counter++} mirrorh: {mirrorh}, mirrorv: {mirrorv}");
    // Console.ForegroundColor = ConsoleColor.White;
    return mirrorh * 100 + mirrorv;
}

long l = lines.GroupBy(l => l.Length == 0 ? sep++ : sep, (k, g) => g.ToArray())
     .Select(g => g.Where(l => l.Length > 0).ToArray())
     .Sum(Mirror);

Console.WriteLine(l);