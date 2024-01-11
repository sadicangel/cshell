using CommandLine;
using CShell.DataModel;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace CShell.Commands.Consumers;

[Verb("table", HelpText = "Renders result as a table.")]

public sealed class ToTable : IConsumerCommand
{
    public void Execute(ShellContext context, ShellObject @object)
    {
        var value = @object.Switch(
            scalar => new Table().BorderColor(Color.Grey).AddColumn("Value").AddRow(scalar.ToStringSafe()),
            record => new Table().BorderColor(Color.Grey).AddColumns(record.Keys.ToArray()).AddRow(record.Values.Select(ShellExtensions.ToStringSafe).ToArray()),
            array => GetRenderableForCollection(array.AsEnumerable()),
            () => new Text("empty\n", "grey"));

        context.Console.Write(value);
    }

    private static IRenderable GetRenderableForCollection(IEnumerable<ShellObject> objects)
    {
        return objects.Switch<IRenderable>(
            scalars =>
            {
                var table = new Table().BorderColor(Color.Grey).AddColumn("Value");
                foreach (var scalar in scalars)
                    table.AddRow(scalar.ToStringSafe());
                return table;
            },
            records =>
            {
                var table = new Table().BorderColor(Color.Grey).AddColumns(records.First().Keys.ToArray());
                foreach (var record in records)
                    table.AddRow(record.Values.Select(ShellExtensions.ToStringSafe).ToArray());
                return table;
            },
            arrays =>
            {
                var table = new Table().BorderColor(Color.Grey).AddColumn("Array");
                foreach (var array in arrays)
                    table.AddRow(array.ToStringSafe());
                return table;
            },
            () => new Text("empty\n", "grey"));
    }
}
