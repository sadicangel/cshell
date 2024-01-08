using CommandLine;
using CShell.DataModel;
using Spectre.Console;

namespace CShell.Commands.Consumers;

[Verb("table", HelpText = "Renders result as a table.")]

public sealed class ToTable : IConsumerCommand
{
    public void Execute(ShellContext context, IEnumerable<ShellObject> objects)
    {
        if (objects.FirstOrDefault() is not ShellObject first)
        {
            context.Console.MarkupLine("[grey i]empty[/]");
            return;
        }

        var table = new Table().BorderColor(Color.Grey);

        switch (first)
        {
            case ShellScalar o:
                table.AddColumn("Value");
                foreach (var obj in objects.Cast<ShellScalar>())
                    table.AddRow(o.ToStringSafe());
                break;

            case ShellArray o:
                table.AddColumn("Array");
                foreach (var obj in objects.Cast<ShellArray>())
                    table.AddRow(obj.ToStringSafe());
                break;

            case ShellRecord o:
                table.AddColumns(o.Keys.ToArray());
                foreach (var obj in objects.Cast<ShellRecord>())
                    table.AddRow(obj.Values.Select(ShellExtensions.ToStringSafe).ToArray());
                break;
        }

        context.Console.Write(table);
    }
}
