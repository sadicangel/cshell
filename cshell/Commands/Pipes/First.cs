using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Pipes;

[Verb("first", HelpText = "Return the first element.")]
public sealed class First : IPipeCommand
{
    [Value(0, HelpText = "Left operand")]
    public required string Left { get; init; }

    [Value(1, HelpText = "Operator")]
    public required string Operator { get; init; }

    [Value(2, HelpText = "Right operand")]
    public required string Right { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object)
    {
        if (@object is not ShellArray array)
            return @object;

        var predicate = (Left, Operator, Right) switch
        {
            (string left, string @operator, string right) => Operations.CreatePredicate(left, @operator, right),
            (null, null, null) => Operations.IncludeAll,
            _ => throw new InvalidOperationException($"Invalid expression {Left} {Operator} {Right}")
        };

        return array.AsEnumerable().FirstOrDefault(predicate) ?? ShellScalar.Null;
    }
}
