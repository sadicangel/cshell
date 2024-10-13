using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("date", HelpText = "Create a UTC datetime.")]
public sealed class Date : IProducerCommand
{
    [Option('u', "utc")]
    public bool Utc { get; init; }

    [Option('f', "format", HelpText = "Displays the date and time in the Microsoft .NET Framework format indicated by the format specifier.")]
    public string? Format { get; init; }

    [Option('Y', "year", HelpText = "Specifies the year. Min = 1, Max = 9999")]
    public int? Year { get; init; }

    [Option('M', "month", HelpText = "Specifies the month. Min = 1, Max = 12")]
    public int? Month { get; init; }

    [Option('D', "day", HelpText = "Specifies the day. Min = 1, Max = 31")]
    public int? Day { get; init; }

    [Option('h', "hour", HelpText = "Specifies the hour. Min = 0, Max = 23")]
    public int? Hour { get; init; }

    [Option('m', "minute", HelpText = "Specifies the minute. Min = 0, Max = 59")]
    public int? Minute { get; init; }

    [Option('s', "second", HelpText = "Specifies the second. Min = 0, Max = 59")]
    public int? Second { get; init; }

    public ShellObject Execute(ShellContext context)
    {
        var now = Utc ? DateTime.UtcNow : DateTime.Now;

        return new ShellScalar(new DateTime(
            year: Year ?? now.Year,
            month: Month ?? now.Month,
            day: Day ?? now.Day,
            hour: Hour ?? now.Hour,
            minute: Minute ?? now.Minute,
            second: Second ?? now.Second,
            kind: Utc ? DateTimeKind.Utc : DateTimeKind.Local));
    }
}
