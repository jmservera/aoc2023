namespace day2.part2.test;

using day2.part2;

public class Game2Test
{
    [Fact]
    public void PowerTest()
    {
        var game = new Game2("test.txt");
        Assert.Equal(2286, game.MultiplyMinPossible());
    }

    [Fact]
    public void PowerTestInput()
    {
        var game = new Game2("input.txt");
        Assert.Equal(65371, game.MultiplyMinPossible());
    }
}