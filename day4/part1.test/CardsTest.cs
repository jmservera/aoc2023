namespace day4.part1.test;
using day4.part1;
public class CardsTest
{
    [Fact]
    public void IndividualCardsTest()
    {
        int[] expected = { 8, 2, 2, 1, 0, 0 };
        var cards = new Cards("test.txt");
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], cards.GetScore(i + 1));
        }
    }

    [Fact]
    public void IndividualCardsCountTest()
    {
        int[] expected = { 4, 2, 2, 1, 0, 0 };
        var cards = new Cards("test.txt");
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], cards.CountCards(i + 1));
        }
    }

    [Fact]
    public void FullScoreTest()
    {
        var cards = new Cards("test.txt");
        Assert.Equal(13, cards.FullScore());
    }
}