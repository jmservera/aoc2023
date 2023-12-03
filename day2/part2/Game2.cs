using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using day2.part1;
namespace day2.part2;
public class Game2 : Game
{
    public Game2(string fileName) : base(fileName) { }

    public int MultiplyMinPossible()
    {
        int powerSum = 0;
        foreach (var game in this.Games)
        {
            int[] rgb = new int[3];
            for (int i = 0; i < game.Value.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rgb[j] = Math.Max(rgb[j], game.Value[i][j]);
                }
            }
            powerSum += rgb[0] * rgb[1] * rgb[2];
        }
        return powerSum;
    }
}
