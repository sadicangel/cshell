using CommandLine;
using Spectre.Console;

namespace CShell.Commands;

[Verb("table", HelpText = "Renders result as a table.")]
public sealed class ToTable : IConsumerCommand
{
    public int Execute(ShellContext context, IEnumerable<Record> records)
    {
        var table = new Table();

        if (records.FirstOrDefault() is Record first)
        {
            table.AddColumns(first.Keys.ToArray());

            foreach (var record in records)
                table.AddRow(record.Values.Select(v => v?.ToString() ?? string.Empty).ToArray());
        }

        context.Console.Write(table);

        return 0;
    }
}
