
var lines = File.ReadAllLines("input.txt");
var result = lines.Select(line => line.Split(' '))
                  .Select(parts => new
                  {
                      hand = parts[0].Replace('A', 'Z').Replace('K', 'Y').Replace('Q', 'X').Replace('J', 'W').Replace('T', 'V'),
                      bet = int.Parse(parts[1]),
                      handType = parts[0].GroupBy(c => c).Sum(g => g.Count() * g.Count())
                  })
                  .OrderBy(r => r.handType).ThenBy(r => r.hand)
                  .Select((r, i) => new { r.hand, r.bet, r.handType, rank = i + 1 })
                  .Aggregate(0, (a, b) => a + b.bet * b.rank);

Console.WriteLine(result);


