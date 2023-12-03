namespace day3.part2.test;

public class MapParts2Test
{
    [Fact]
    public void MultiplyTest()
    {
        var map = new MapParts2("test.txt");
        Assert.Equal(467835, map.MultiplyGears());
    }
}