using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Pipes;

[Verb("select", HelpText = "Project a new value.")]
public sealed class Select : IPipeCommand
{
    [Value(0, HelpText = "Select expression", Required = true)]
    public required string Expression { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object)
    {
        return @object.Switch(
            scalar => scalar,
            record => record.EvaluateExpression(Expression),
            array => new ShellArray(array.Select(e => e.EvaluateExpression(Expression))));
    }
}