namespace part2;
public static class Calibration
{
    public static int Calculate(string[] input)
    {
        int result = 0;
        foreach (string line in input)
        {
            var numbers = line.Where(Char.IsDigit).ToArray();
            result += (numbers.First() - '0') * 10 + numbers.Last() - '0';
        }
        return result;
    }
}