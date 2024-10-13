using CommandLine;
using CShell.DataModel;
using CShell.Parsing;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace CShell.Commands.Consumers;

[Verb("table", HelpText = "Renders result as a table.")]

public sealed class ToTable : IConsumerCommand
{
    public void Execute(ShellContext context, ShellObject @object)
    {
        var value = @object.Switch(
            scalar => new Table().BorderColor(Color.Grey).AddColumn("Value").AddRow(scalar.ToString()),
            array => GetRenderableForCollection(array.AsEnumerable()),
            record => new Table().BorderColor(Color.Grey).AddColumns(record.Keys.ToArray()).AddRow(record.Values.Select(r => r.ToString()).ToArray()));

        context.Console.Write(value);
    }

    private static IRenderable GetRenderableForCollection(IEnumerable<ShellObject> objects)
    {
        var columns = new SortedSet<string>();
        var rows = new List<Dictionary<string, string>>();

        foreach (var @object in objects)
        {
            var row = @object.Switch(
                s => new Dictionary<string, string> { ["Value"] = s.ToString() },
                a => a.Select((o, i) => (Object: o, Index: i)).ToDictionary(e => e.Index.ToString(), e => e.Object.ToString()),
                r => r.ToDictionary(e => e.Key, e => e.Value.ToString()));

            columns.UnionWith(row.Keys);
            rows.Add(row);
        }

        if (columns.Count is 0)
            return new Text("empty\n", "grey");

        var table = new Table().BorderColor(Color.Grey).AddColumns(columns.ToArray());
        foreach (var row in rows)
            table.AddRow(columns.Select(c => row.GetValueOrDefault(c, string.Empty)).ToArray());
        return table;
    }
}
