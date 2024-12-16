using args.console;

var arguments = Parser.Parse(args);

if (arguments.HasErrors)
{
    Console.WriteLine("Unknown argument(s):");
    var pars = arguments.UnknownParameters.Aggregate("", (p, p1) => $"{p} {p1}, ");
    var switches = arguments.UnknownSwitches.Aggregate("", (s, s1) => $"{s} {s1}, ");

    if (!string.IsNullOrWhiteSpace(pars))
    {
        Console.WriteLine($"Parameters: {pars}");
    }

    if (!string.IsNullOrWhiteSpace(switches))
    {
        Console.WriteLine($"Switches: {switches}");
    }

    return;
}

Console.WriteLine("Arguments were:");
Console.WriteLine(arguments.ToString());
