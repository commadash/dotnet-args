using System.Text;

namespace args.console;

/// <summary>
/// Generated at 16.12.2024 21.43.48
/// </summary>
public class Arguments
{
    public bool Parallel { get; set; }
    public bool List { get; set; }
    public bool Force { get; set; }

    public string? Server { get; set; }
    public string? Path { get; set; }

    public IList<string> UnknownSwitches { get; set; } = [];
    public IList<string> UnknownParameters { get; set; } = [];
    public bool HasErrors => UnknownSwitches.Any() || UnknownParameters.Any();

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Parallel:{Parallel}");
        builder.AppendLine($"List:{List}");
        builder.AppendLine($"Force:{Force}");
        builder.AppendLine($"Server:{Server ?? "null"}");
        builder.AppendLine($"Path:{Path ?? "null"}");

        return builder.ToString();
    }
}
