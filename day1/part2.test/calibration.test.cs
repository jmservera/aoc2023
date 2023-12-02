namespace part2.test;

public class CalibrationTest
{
    [Fact]
    public void Calculate_Returns_281_For_Test()
    {
        // load test file lines
        var input = System.IO.File.ReadAllLines("test.txt");
        // Arrange
        // Act
        var result = Calibration.Calculate(input);

        // Assert
        Assert.Equal(281, result);
    }
}