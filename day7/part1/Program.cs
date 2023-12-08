﻿(string hand, int bet, int handType) calcHand(string[] parts, int partIndex)
{
    if (partIndex == 1)
    {
        return (
             parts[0].Replace('A', 'Z').Replace('K', 'Y').Replace('Q', 'X').Replace('J', 'W').Replace('T', 'V'),
             int.Parse(parts[1]),
            parts[0].GroupBy(c => c).Sum(g => g.Count() * g.Count())
        );
    }
    else
    {
        var handCalc = parts[0];
        if (parts[0].Contains('J'))
        {
            var r = parts[0].Replace('J', '0').GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Where(g => g != '0')
                .FirstOrDefault();
            if (r != 0)
                handCalc = parts[0].Replace('J', r);
        }
        return (
         parts[0].Replace('A', 'Z').Replace('K', 'Y').Replace('Q', 'X').Replace('J', '0').Replace('T', 'V'),
         int.Parse(parts[1])
         , handCalc.GroupBy(c => c).Sum(g => g.Count() * g.Count())
        );
    }
}

int getResult(string[] lines, int partIndex)
{
    return lines.Select(line => line.Split(' '))
                .Select(parts => calcHand(parts, partIndex))
                .OrderBy(r => r.handType).ThenBy(r => r.hand)
                .Select((r, i) => new { r.hand, r.bet, r.handType, rank = i + 1 })
                .Aggregate(0, (a, b) => a + b.bet * b.rank);
}

var lines = File.ReadAllLines("input.txt");
var result = getResult(lines, 1);

Console.WriteLine(result);

var result2 = getResult(lines, 2);

Console.WriteLine(result2);

