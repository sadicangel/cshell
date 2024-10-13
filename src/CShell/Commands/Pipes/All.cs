using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Pipes;

public sealed class All : IPipeCommand
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

        return new ShellScalar(array.AsEnumerable().All(predicate));
    }
}
