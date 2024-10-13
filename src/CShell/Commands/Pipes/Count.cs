using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Pipes;

[Verb("count", HelpText = "Returns the number of results.")]

public sealed class Count : IPipeCommand
{
    public ShellObject Execute(ShellContext context, ShellObject @object) =>
        new ShellScalar(@object.Switch(s => 1, a => a.Length, r => r.Count));
}
