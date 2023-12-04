using System.Security.Cryptography.X509Certificates;

namespace day4.part1;

public class Cards
{
    public Dictionary<int, (int[], int[])> cards = new Dictionary<int, (int[], int[])>();

    public (int[], int[]) GetCard(int id)
    {
        return cards[id];
    }

    public Cards(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        foreach (var line in lines)
        {
            var parts = line.Split(": ");
            var id = int.Parse(parts[0][4..]);
            var cardParts = parts[1].Split(" | ");
            var left = cardParts[0].Chunk(3).Select(s => new string(s)).Select(x => int.Parse(x)).ToArray();
            var right = cardParts[1].Chunk(3).Select(s => new string(s)).Select(x => int.Parse(x)).ToArray();
            cards.Add(id, (left, right));
        }
    }

    public int CountCards(int id)
    {
        var (left, right) = cards[id];
        var count = right.Where(x => left.Contains(x)).Count();

        return count;
    }

    public int GetScore(int id)
    {
        var count = CountCards(id);
        return count == 0 ? 0 : 1 << (count - 1);
    }

    public int FullScore()
    {
        return cards.Keys.Select(x => GetScore(x)).Sum();
    }

    public int CalculateStack()
    {
        int[] multipliers = new int[cards.Count];
        for (int i = 0; i < multipliers.Length; i++)
        {
            multipliers[i] = 1;
        }
        for (int i = 0; i < multipliers.Length; i++)
        {
            var c = CountCards(i + 1);
            for (int j = 0; j < c; j++)
            {
                multipliers[i + j + 1] += multipliers[i];
            }
        }

        return multipliers.Sum();
    }

}
