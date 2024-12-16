using System.Text.RegularExpressions;

namespace args.console;

public class Parser
{
    /// <summary>
    /// Parses incoming list of arguments.
    ///
    /// The arguments can be either flags or argument followed by a value
    ///
    /// So there are two separate expressions that results in an assignment:
    ///
    ///     "-switch" -> property = true
    ///     "-param value" -> property = value
    ///
    /// The parsed result is returned as an instance of the <see cref="Arguments"/> class.
    /// Check the UnknownParameters and UnknownSwitches properties on the Arguments object for errors.
    ///
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Arguments Parse(string[] parameters)
    {
        var joinedString = parameters.Aggregate("", (s1, s2) => s1 + " " + s2);
        var pattern = @"(?<key>-\w+) (?<value>[^-]+)|(?<flag>-\w)(?: |$)";

        var regex = new Regex(pattern, RegexOptions.Compiled);
        var matches = regex.Matches(joinedString);

        var switches = matches
            .Where(m => m.Groups["flag"].Success)
            .Select(m => m.Groups["flag"].Value);
        var properties = matches
            .Where(m => m.Groups["key"].Success && m.Groups["value"].Success)
            .ToDictionary(m => m.Groups["key"].Value, m => m.Groups["value"].Value.TrimEnd());

        var arguments = ArgumentsFactory.Create(switches, properties);

        var unknownSwitches = regex
            .Replace(joinedString, "")
            .Split()
            .Where(s => !string.IsNullOrEmpty(s));
        arguments.UnknownSwitches = [.. arguments.UnknownSwitches, .. unknownSwitches];

        return arguments;
    }
}
