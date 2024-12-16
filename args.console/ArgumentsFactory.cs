namespace args.console;

public static class ArgumentsFactory
{
    public static Arguments Create(
        IEnumerable<string> switches,
        IDictionary<string, string> properties
    )
    {
        var arguments = new Arguments();

        arguments.AddSwitches(switches);
        arguments.AddProperties(properties);

        return arguments;
    }
}
