using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("cd", HelpText = "Change directory.")]
public sealed class Cd : IProducerCommand
{
    [Value(0, HelpText = "The path to change to.", Required = true)]
    public required string Path { get; init; }

    public ShellObject Execute(ShellContext context)
    {
        var path = System.IO.Path.GetFullPath(Path);

        if (!Directory.Exists(path))
            throw new CommandException(nameof(Cd), $"Cannot find path '{path}' because it does not exist.");

        context.CurrentDirectory = path;

        return new ShellScalar(path);
    }
}

