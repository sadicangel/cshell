using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Pipes;

[Verb("count", HelpText = "Returns the number of results.")]

public sealed class Count : IPipeCommand
{
    public IEnumerable<ShellObject> Execute(ShellContext context, IEnumerable<ShellObject> objects)
    {
        return [new ShellScalar(objects.Count())];
    }
}