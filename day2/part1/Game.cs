namespace day2.part1;

public class Game
{
    public Dictionary<int, int[][]> Games { get; } = new Dictionary<int, int[][]>();
    public Game(string fileName)
    {
        ParseGame(fileName);
    }

    void ParseGame(string fileName)
    {
        string[] lines = System.IO.File.ReadAllLines(fileName);
        foreach (string line in lines)
        {
            var game = line.Split(':');
            var key = int.Parse(game[0].Substring(5));
            var turns = game[1].Split(';');
            int[][] turnInfo = new int[turns.Length][];
            int i = 0;
            foreach (var turn in turns)
            {
                turnInfo[i] = new int[3]; // RGB
                var turnValues = turn.Split(',');
                foreach (var color in turnValues)
                {
                    var colorInfo = color.Trim().Split(' ');
                    var nr = int.Parse(colorInfo[0]);
                    switch (colorInfo[1])
                    {
                        case "red":
                            turnInfo[i][0] = nr;
                            break;
                        case "green":
                            turnInfo[i][1] = nr;
                            break;
                        case "blue":
                            turnInfo[i][2] = nr;
                            break;
                    }
                }
                i++;
            }
            Games.Add(key, turnInfo);
        }
    }

    public int SumPossible(int[] rgb)
    {
        int sum = 0;
        foreach (var game in Games)
        {
            if (IsPossible(game.Value, rgb))
            {
                sum += game.Key;
            }
        }
        return sum;
    }

    public bool IsPossible(int[][] turns, int[] rgb)
    {
        for (int i = 0; i < turns.Length; i++)
        {
            if (turns[i][0] > rgb[0] || turns[i][1] > rgb[1] || turns[i][2] > rgb[2])
            {
                return false;
            }
        }
        return true;
    }
}