namespace day3.part1.test;
using Xunit.Abstractions;
public class MapTest
{
    private readonly ITestOutputHelper output;

    public MapTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void TestLoad()
    {
        var map = new MapParts("test.txt");
        Assert.Equal(10, map.Width);
        Assert.Equal(10, map.Height);
        var m = map.ToString();
        Assert.Equal(110, m.Length);
    }

    [Fact]
    public void TestSum()
    {
        var map = new MapParts("test.txt");
        Assert.Equal(4361, map.SumSerials());
    }
}