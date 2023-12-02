namespace part1.test;

public class CalibrationTest
{
    [Fact]
    public void Calculate_Returns_142_For_Test()
    {
        // load test file lines
        var input = System.IO.File.ReadAllLines("test.txt");
        // Arrange
        // Act
        var result = Calibration.Calculate(input);

        // Assert
        Assert.Equal(142, result);
    }
}