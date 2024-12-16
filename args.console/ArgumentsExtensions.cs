namespace args.console;

public static class ArgumentsExtensions
{
    public static void AddProperties(
        this Arguments arguments,
        IDictionary<string, string> properties
    )
    {
        var argType = typeof(Arguments);
        foreach (var property in properties)
        {
            if (ArgumentMappings.Parameters.TryGetValue(property.Key, out var propertyName))
            {
                var pi = argType.GetProperty(propertyName);
                pi?.SetValue(arguments, property.Value);
            }
            else
            {
                var pi = argType.GetProperty("UnknownParameters");
                var unknownParameters = pi?.GetValue(arguments, null) as List<string>;
                unknownParameters?.Add($"{property.Key} {property.Value}");
            }
        }
    }

    public static void AddSwitches(this Arguments arguments, IEnumerable<string> switches)
    {
        var argType = typeof(Arguments);
        switches
            .ToList()
            .ForEach(name =>
            {
                if (ArgumentMappings.Switches.TryGetValue(name, out var propertyName))
                {
                    var pi = argType.GetProperty(propertyName);
                    pi?.SetValue(arguments, true);
                }
                else
                {
                    var pi = argType.GetProperty("UnknownSwitches");
                    var unknownSwitches = pi?.GetValue(arguments, null) as List<string>;
                    unknownSwitches?.Add(name);
                }
            });
    }
}
