namespace MarlinConsole.Tests;

public class GCodeParserTests
{
    private GCodeParser parser;

    public GCodeParserTests()
    {
        parser = new GCodeParser();
    }

    [Theory]
    [InlineData("X:0.0000 Y:0.0000 Z:275.5441 E:0.0000 Count A:81684B:81684C:81684", "Z", "275.5441")]
    [InlineData("X:0.0000 Y:0.0000 Z:275.5441 E:0.0000 Count A:81684B:81684C:81684", "X", "0.0000")]
    [InlineData("X:0.0000 Y:0.0000 Z:275.5441 E:0.0000 Count A:81684B:81684C:81684", "A", "81684")]
    public void Test1(string code, string parameter, string expected)
    {
        var actual = parser.ExtractValue(code, parameter);
        actual.Should().Be(expected);
    }
}