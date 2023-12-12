
var lines = File.ReadAllLines("input.txt");

int counter = 0;

var result = lines.Select(line => line.Split(' '))
        .Select(line => (pattern: line[0], values: line[1].Split(',').Select(int.Parse).ToArray()))
        .AsParallel()
        .Sum(line =>
        {
            var i = Interlocked.Increment(ref counter);
            Console.WriteLine($"${i}: {line.pattern} {string.Join(',', line.values)}");
            var r = Count(line.pattern, line.values);
            Console.WriteLine($"${i}: {line.pattern} {string.Join(',', line.values)} {r}");
            return r;
        });


long Count(string pattern, int[] values, bool first = true)
{
    if (pattern.Length < values.Sum() + values.Length - 1)
        return 0;
    long result = 0L;
    if (pattern.Length == 0)
    {
        if (values.Length == 0 || (values.Length == 1 && values[0] == 0))
        {
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
            return 1;
        }
    }

    if (values[0] == 0)
    {
        if (pattern[0] == '#')
            return 0;
        else
            result += Count(pattern[1..], values[1..], true);
    }
    else
    {
        if (pattern[0] == '.')
        {
            if (first)
                result += Count(pattern[1..], values, first);
            else
                return 0;
        }
        else
        {
            if (pattern[0] == '?' && first)
            {
                result += Count(pattern[1..], values, true);
            }
            var newValues = new int[values.Length];
            values.CopyTo(newValues, 0);
            newValues[0]--;
            result += Count(pattern[1..], newValues, false);
        }
    }

    return result;
}
Console.WriteLine(result);

counter = 0;

long unfold = lines.Select(line => line.Split(' '))
        .Select(line => (pattern: line[0], values: line[1].Split(',').Select(int.Parse).ToArray()))
        .Select(line => (pattern: String.Join('?', Enumerable.Repeat<string>(line.pattern, 5).ToArray()), values: Enumerable.Repeat(line.values, 5).SelectMany(x => x).ToArray(), result: Count(line.pattern, line.values)))
        .AsParallel()
        .WithDegreeOfParallelism(160)
        .Sum(line =>
        {
            var i = Interlocked.Increment(ref counter);
            // Console.WriteLine($"${i}: {line.pattern} {string.Join(',', line.values)}");
            var r = Count(line.pattern, line.values);
            Console.WriteLine($"${i}: {line.pattern} {string.Join(',', line.values)} {r}");
            return r;
        });

Console.WriteLine(unfold);
