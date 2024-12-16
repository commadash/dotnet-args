using args.console;

namespace args.test;

public class ParserTest
{
    /// <summary>
    /// Test that calling the method returns an empty <see cref="Arguments"/> instance.
    /// </summary>
    [Fact]
    public void TestParse()
    {
        var arguments = Parser.Parse([]);
        Assert.NotNull(arguments);
        Assert.Equivalent(new Arguments(), arguments);
    }

    /// <summary>
    /// A list of known flags should return correct <see cref="Arguments"/> object.
    /// </summary>
    [Fact]
    public void GivenA_NumberOfFlags_WhenParseIsCalled_ThenCorrectArgumentsAreReturned()
    {
        string[] flags = ["-p", "-f", "-1", "-h"];

        var actual = Parser.Parse(flags);

        string[] expectedUnknownSwitches = ["-1", "-h"];
        var expected = new Arguments()
        {
            List = false,
            Parallel = true,
            Force = true,
            UnknownSwitches = expectedUnknownSwitches
        };

        Assert.Equivalent(expected, actual);
        Assert.Equivalent(expectedUnknownSwitches, actual.UnknownSwitches);
    }

    /// <summary>
    /// A list of known parameters should return correct <see cref="Arguments"/> object.
    /// </summary>
    [Fact]
    public void GivenAnArrayOfParameters_WhenParseIsCalled_ThenCorrectArgumentsAreReturned()
    {
        string[] paramsArray = ["-path", "/some/path/"];

        var actual = Parser.Parse(paramsArray);

        var expected = new Arguments() { Path = "/some/path/" };
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public void GivenArgsWithSwitchesAndParameters_WhenParsed_ThenCorrectArgumentsReturned()
    {
        string[] args =
        [
            "kloo",
            "-p",
            "-path",
            "/some/path/",
            "-l",
            "-k",
            "-johnny",
            "jackie",
            "-2",
            "-32"
        ];

        var actual = Parser.Parse(args);

        List<string> expectedUnknownSwitches = ["kloo", "-k", "-2", "-32"];
        List<string> expectedUnknownParameters = ["-johnny jackie"];
        var expected = new Arguments()
        {
            Path = "/some/path/",
            List = true,
            Force = false,
            Parallel = true,
            UnknownParameters = expectedUnknownParameters,
            UnknownSwitches = expectedUnknownSwitches
        };
        Assert.Equivalent(expected, actual);
        Assert.Equivalent(expectedUnknownParameters, actual.UnknownParameters);
        Assert.Equivalent(expectedUnknownSwitches, actual.UnknownSwitches);
    }
}
