using CommandLine;

namespace CShell.Commands.Pipes;

[Verb("count", HelpText = "Returns the number of results.")]

public sealed class Count : IPipeCommand
{
    public IEnumerable<Record> Execute(ShellContext context, IEnumerable<Record> records)
    {
        var record = new Record(new Dictionary<string, object?>
        {
            ["Count"] = records.Count(),
        });

        return [record];
    }
}