using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Pipes;

[Verb("count", HelpText = "Returns the number of results.")]

public sealed class Count : IPipeCommand
{
    public ShellObject Execute(ShellContext context, ShellObject @object) =>
        new ShellScalar(@object.Switch(scalar => 1, array => array.Length, record => record.Count));
}
