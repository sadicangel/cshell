using CommandLine;

namespace CShell.Commands.Producers;

[Verb("cd", HelpText = "Change directory.")]
public sealed class Cd : IProducerCommand
{
    [Value(0, HelpText = "The path to change to.", Required = true)]
    public required string Path { get; init; }

    public IEnumerable<Record> Execute(ShellContext context)
    {
        var path = System.IO.Path.GetFullPath(Path);

        if (!Directory.Exists(path))
            throw new CommandException(nameof(Cd), $"Cannot find path '{path}' because it does not exist.");

        context.CurrentDirectory = path;

        var record = new Record(new Dictionary<string, object?>
        {
            ["CurrentDirectory"] = path,
        });

        return [record];
    }
}
