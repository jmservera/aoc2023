using System.Text.RegularExpressions;

namespace part2;
public static class Calibration
{
    public static int Calculate(string[] input)
    {
        int result = 0;
        foreach (string line in input)
        {
            var nums = ConvertNumbers(line);

            var lastNumberMatch = Regex.Match(nums, @"(\d)(?!.*\d)");
            if (lastNumberMatch.Success)
            {
                var lastNumber = lastNumberMatch.Groups[1].Value[0] - '0';

                var firstNumberMatch = Regex.Match(nums, @"(\d)", RegexOptions.None);
                if (firstNumberMatch.Success)
                {
                    var firstNumber = firstNumberMatch.Groups[1].Value[0] - '0';
                    result += firstNumber * 10 + lastNumber;
                }
            }
        }
        return result;
    }

    private static string ConvertNumbers(string line)
    {
        var list = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        var matches = Regex.Matches(line, $"(?=(${string.Join("|", list)}))");
        // replace each string with its index in the list
        foreach (Match match in matches)
        {
            var index = Array.IndexOf(list, match.Groups[1].Value);
            line = line.Remove(match.Index, 1).Insert(match.Index, $"{index}");
        }

        return line;
    }
}