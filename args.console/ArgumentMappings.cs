namespace args.console;

/// <summary>
/// Generated at 16.12.2024 21.43.48
/// </summary>
public static class ArgumentMappings
{
    public static readonly IDictionary<string, string> Switches = new Dictionary<string, string>()
    {
        { "-p", "Parallel" },
        { "-l", "List" },
        { "-f", "Force" },
    };
    public static readonly IDictionary<string, string> Parameters = new Dictionary<string, string>()
    {
        { "-server", "Server" },
        { "-path", "Path" },
    };
}
