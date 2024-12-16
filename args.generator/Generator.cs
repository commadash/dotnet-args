using System.Text;
using System.Text.Json;

namespace args.generator;

public class Generator
{
    public static void Run()
    {
        Console.WriteLine("generating c# files...");

        var file = File.ReadAllText("args.generator/mappings.json");

        var mappings = JsonSerializer.Deserialize<Mappings>(file);

        GenerateArguments(mappings);

        GenerateArgumentMappings(mappings);

        Console.WriteLine("c# files generated.");
    }

    private static void GenerateArgumentMappings(Mappings? mappings)
    {
        if (mappings == null)
            return;

        var path = $"{mappings.Filepath}/ArgumentMappings.cs";

        var builder = new StringBuilder(
            $@"namespace args.console;

/// <summary>
/// Generated at {DateTime.Now}
/// </summary>
public static class ArgumentMappings
{{
"
        );
        builder.Append(
            @"    public static readonly IDictionary<string, string> Switches = new Dictionary<string, string>()"
        );

        var switches = mappings.Switches ?? new Dictionary<string, string>();
        if (switches.Any())
        {
            builder.AppendLine($"{Environment.NewLine}    {{");

            foreach (var switchPair in switches)
            {
                builder.AppendLine(
                    $$"""        { "-{{switchPair.Key}}", "{{switchPair.Value}}" },"""
                );
            }
            builder.AppendLine("    };");
        }
        else
        {
            builder.AppendLine(";");
        }
        builder.Append(
            @"    public static readonly IDictionary<string, string> Parameters = new Dictionary<string, string>()"
        );

        var parameters = mappings.Parameters ?? new Dictionary<string, string>();
        if (parameters.Any())
        {
            builder.AppendLine($"{Environment.NewLine}    {{");
            foreach (var param in parameters)
            {
                builder.AppendLine($$"""        { "-{{param.Key}}", "{{param.Value}}" },""");
            }
            builder.AppendLine("    };");
        }
        else
        {
            builder.AppendLine(";");
        }
        builder.AppendLine("}");

        File.WriteAllText(path, builder.ToString());
    }

    private static void GenerateArguments(Mappings? mappings)
    {
        if (mappings == null)
            return;

        var path = $"{mappings.Filepath}/Arguments.cs";
        Console.WriteLine(path);
        var argumentBuilder = new StringBuilder(
            $@"using System.Text;

namespace args.console;

/// <summary>
/// Generated at {DateTime.Now}
/// </summary>
public class Arguments
{{
"
        );
        argumentBuilder.AppendLine(AddSwitches(mappings.Switches?.Values.ToList()));
        argumentBuilder.AppendLine(AddParameters(mappings.Parameters?.Values.ToList()));
        argumentBuilder.AppendLine(
            @"    public IList<string> UnknownSwitches { get; set; } = [];
    public IList<string> UnknownParameters { get; set; } = [];
    public bool HasErrors => UnknownSwitches.Any() || UnknownParameters.Any();

    public override string ToString()
    {
        var builder = new StringBuilder();"
        );
        argumentBuilder.AppendLine(AddToString(mappings));
        argumentBuilder.AppendLine(
            @"        return builder.ToString();
    }
}"
        );
        File.WriteAllText(path, argumentBuilder.ToString());
    }

    private static string AddSwitches(List<string>? switches)
    {
        return switches
                ?.Select(s => $"    public bool {s} {{ get; set; }}")
                .Aggregate("", (s1, s2) => $"{s1}{s2}\n") ?? "";
    }

    private static string AddParameters(List<string>? parameters)
    {
        return parameters
                ?.Select(p => $"    public string? {p} {{ get; set; }}")
                .Aggregate("", (s1, s2) => $"{s1}{s2}\n") ?? "";
    }

    private static string AddToString(Mappings mappings)
    {
        var switches =
            mappings
                ?.Switches?.Values.Select(s =>
                    $$"""        builder.AppendLine($"{{s}}:{{{s}}}");"""
                )
                .Aggregate("", (s1, s2) => $"{s1}{s2}\n") ?? "";
        var parameters =
            mappings
                ?.Parameters?.Values.Select(p =>
                    $$"""        builder.AppendLine($"{{p}}:{{{p}} ?? "null"}");"""
                )
                .Aggregate("", (s1, s2) => $"{s1}{s2}\n") ?? "";

        return $"{switches}{parameters}";
    }
}
