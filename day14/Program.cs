var lines = File.ReadAllLines("input.txt");

bool show = false;
char[][] tilted = lines.Select(s => s.ToCharArray()).ToArray();
for (int i = 0; i < lines.Length - 1; i++)
{
    for (int n = 0; n < lines.Length - 1 - i; n++)
    {
        for (int j = 0; j < lines[i].Length; j++)
        {
            if (tilted[n][j] == '.' && tilted[n + 1][j] == 'O')
            {
                tilted[n][j] = 'O';
                tilted[n + 1][j] = '.';
            }
        }
    }
    if (show)
    {
        int c = 0;
        tilted.ToList().ForEach(a =>
        {
            Console.Write(new string(a));
            if (c++ == i)
                Console.WriteLine("  <");
            else
                Console.WriteLine("   ");
        });
        Console.SetCursorPosition(0, Console.CursorTop - lines.Length);
        Thread.Sleep(1000);
    }
}


tilted.ToList().ForEach(a =>
            Console.WriteLine(new string(a)));

long sum = tilted.Select((a, i) => (a, i)).Sum(a => a.a.Count(b => b == 'O') * (tilted.Length - a.i));
Console.WriteLine(sum);

Console.WriteLine((tilted.Length, tilted[0].Length));

