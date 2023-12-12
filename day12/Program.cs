
var lines = File.ReadAllLines("input.txt");

var result = lines.Select(line => line.Split(' '))
        .Select(line => (pattern: line[0], values: line[1].Split(',').Select(int.Parse).ToArray()))
        .Sum(line =>
        {
            Console.WriteLine($"{line.pattern} {string.Join(',', line.values)}");
            var r = Count(line.pattern, line.values);
            Console.WriteLine($"{line.pattern} {string.Join(',', line.values)} {r}");
            return r;
        });


long Count(string pattern, int[] values, bool first = true, string answer = "")
{
    long result = 0L;
    if (pattern.Length == 0)
    {
        if (values.Length == 0 || (values.Length == 1 && values[0] == 0))
        {
            Console.WriteLine(answer);
            return 1;
        }
        else
            return 0;
    }

    if (values.Length == 0 || (values.Length == 1 && values[0] == 0))
    {
        if (pattern.Contains('#'))
            return 0;
        else
        {
            Console.WriteLine(answer + ".");
            return 1;
        }
    }

    if (values[0] == 0)
    {
        if (pattern[0] == '#')
            return 0;
        else
            result += Count(pattern[1..], values[1..], true, answer + ".");
    }
    else
    {
        if (pattern[0] == '.')
        {
            if (first)
                result += Count(pattern[1..], values, first, answer + ".");
            else
                return 0;
        }
        else
        {
            if (pattern[0] == '?' && first)
            {
                result += Count(pattern[1..], values, true, answer + ".");
            }
            values = values.Clone() as int[];
            values[0]--;
            result += Count(pattern[1..], values, false, answer + "#");
        }
    }

    return result;
}
Console.WriteLine(result);