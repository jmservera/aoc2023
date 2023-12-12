
var lines = File.ReadAllLines("input.txt");

var result = lines.Select(line => line.Split(' '))
        .Select(line => (pattern: line[0], values: line[1].Split(',').Select(int.Parse).ToArray()))
        .Sum(line => Count(line.pattern, line.values));


static long Count(string pattern, int[] values, bool first = true, Dictionary<string, long>? memo = null)
{
    if (pattern.Length < values.Sum() + values.Length - 1)
    {
        return 0;
    }

    memo ??= new Dictionary<string, long>();

    string key = $"{pattern}|{string.Join(",", values)}|{first}";
    if (memo.ContainsKey(key))
    {
        return memo[key];
    }

    long result = 0L;
    if (pattern.Length == 0)
    {
        if (values.Length == 0 || (values.Length == 1 && values[0] == 0))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    if (values.Length == 0 || (values.Length == 1 && values[0] == 0))
    {
        if (pattern.Contains('#'))
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    if (values[0] == 0)
    {
        if (pattern[0] == '#')
        {
            return 0;
        }
        else
        {
            result += Count(pattern[1..], values[1..], true, memo);
        }
    }
    else
    {
        if (pattern[0] == '.')
        {
            if (first)
            {
                result += Count(pattern[1..], values, first, memo);
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (pattern[0] == '?' && first)
            {
                result += Count(pattern[1..], values, true, memo);
            }
            var newValues = new int[values.Length];
            values.CopyTo(newValues, 0);
            newValues[0]--;
            result += Count(pattern[1..], newValues, false, memo);
        }
    }

    memo[key] = result;
    return result;
}
Console.WriteLine(result);

long unfold = lines.Select(line => line.Split(' '))
        .Select(line => (pattern: line[0], values: line[1].Split(',').Select(int.Parse).ToArray()))
        .Select(line => (pattern: String.Join('?', Enumerable.Repeat<string>(line.pattern, 5).ToArray()), values: Enumerable.Repeat(line.values, 5).SelectMany(x => x).ToArray(), result: Count(line.pattern, line.values)))
        .Sum(line => Count(line.pattern, line.values));

Console.WriteLine(unfold);
