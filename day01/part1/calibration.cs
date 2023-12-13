using System.Text.RegularExpressions;

namespace part1;
public static class Calibration
{
    public static int Calculate(string[] input)
    {
        int result = 0;
        foreach (string line in input)
        {
            var lastNumberMatch = Regex.Match(line, @"(\d)(?!.*\d)");
            if (lastNumberMatch.Success)
            {
                var lastNumber = lastNumberMatch.Groups[1].Value[0] - '0';

                var firstNumberMatch = Regex.Match(line, @"(\d)", RegexOptions.None);
                if (firstNumberMatch.Success)
                {
                    var firstNumber = firstNumberMatch.Groups[1].Value[0] - '0';
                    result += firstNumber * 10 + lastNumber;
                }
            }
        }
        return result;
    }
}