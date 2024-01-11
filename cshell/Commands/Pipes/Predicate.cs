using CommandLine;
using CShell.DataModel;
using static CShell.Commands.Operations;

namespace CShell.Commands.Pipes;

public abstract class Predicate : IPipeCommand
{
    [Value(0, HelpText = "Left operand", Required = true)]
    public required string Left { get; init; }

    [Value(1, HelpText = "Operator", Required = true)]
    public required string Operator { get; init; }

    [Value(2, HelpText = "Right operand", Required = true)]
    public required string Right { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object) => Execute(context, @object, CreatePredicate(Operator));

    protected abstract ShellObject Execute(ShellContext context, ShellObject @object, Func<object?, object?, bool> predicate);

    internal static Func<object?, object?, bool> CreatePredicate(string @operator)
    {
        return @operator switch
        {
        // Equality
        ['=', '='] => (l, r) => AreEqual(l, r),
        ['!', '='] => (l, r) => !AreEqual(l, r),
        // Comparison
        ['>'] => (l, r) => Compare(l, r) > 0,
        ['<'] => (l, r) => Compare(l, r) < 0,
        ['>', '='] => (l, r) => Compare(l, r) >= 0,
        ['<', '='] => (l, r) => Compare(l, r) <= 0,
            _ => throw new FormatException($"Operator '{@operator}' is not valid"),
        };
    }
}
