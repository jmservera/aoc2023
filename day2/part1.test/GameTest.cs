namespace day2.part1.test;

using day2.part1;

public class GameTest
{
    [Fact]
    public void ParseTest()
    {
        var game = new Game("test.txt");
        Assert.Equal(5, game.Games.Count);
        Assert.Equal(3, game.Games[1].Length);
        Assert.Equal(3, game.Games[2].Length);
        Assert.Equal(3, game.Games[3].Length);
        Assert.Equal(3, game.Games[4].Length);
        Assert.Equal(2, game.Games[5].Length);

        Assert.Equal(4, game.Games[1][0][0]);
        Assert.Equal(0, game.Games[1][0][1]);
        Assert.Equal(3, game.Games[1][0][2]);

    }

    [Fact]
    public void SumTest()
    {
        var game = new Game("test.txt");
        Assert.Equal(8, game.SumPossible(new int[] { 12, 13, 14 }));
    }
}