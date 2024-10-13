using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Pipes;

[Verb("where", HelpText = "Filter values based on a row condition.")]
public sealed class Where : IPipeCommand
{
    [Value(0, HelpText = "Left operand", Required = true)]
    public required string Left { get; init; }

    [Value(1, HelpText = "Operator", Required = true)]
    public required string Operator { get; init; }

    [Value(2, HelpText = "Right operand", Required = true)]
    public required string Right { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object)
    {
        if (@object is not ShellArray array)
            return @object;

        var predicate = Operations.CreatePredicate(Left, Operator, Right);

        return new ShellArray(array.AsEnumerable().Where(predicate));
    }
}
