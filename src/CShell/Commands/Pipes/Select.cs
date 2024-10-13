using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Pipes;

[Verb("select", HelpText = "Project a new value.")]
public sealed class Select : IPipeCommand
{
    [Value(0, HelpText = "Select expression", Required = true)]
    public required string Expression { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object) => @object.Switch(
        s => s,
        a => new ShellArray(a.Select(e => e.Evaluate(Expression))),
        r => r.Evaluate(Expression));
}
