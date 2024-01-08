using CommandLine;
using Spectre.Console;

namespace CShell.Commands.Consumers;

[Verb("table", HelpText = "Renders result as a table.")]

public sealed class ToTable : IConsumerCommand
{
    public void Execute(ShellContext context, IEnumerable<Record> records)
    {
        if (records.FirstOrDefault() is not Record first)
        {
            context.Console.MarkupLine("[grey i]null[/]");
            return;
        }

        var table = new Table().BorderColor(Color.Grey);
        table.AddColumns(first.Keys.ToArray());

        foreach (var record in records)
            table.AddRow(record.Values.Select(v => v?.ToString() ?? string.Empty).ToArray());
        context.Console.Write(table);

    }
}
