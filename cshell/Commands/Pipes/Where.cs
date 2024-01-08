using CommandLine;
using static CShell.Commands.Operations;

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

    public IEnumerable<Record> Execute(ShellContext context, IEnumerable<Record> records)
    {
        var where = CreateWhere();

        return records.Where(r => where(r.GetValueOrDefault(Left), Right));
    }

    private Func<object?, object?, bool> CreateWhere()
    {
        return Operator switch
        {
        // Equality
        ['=', '='] => (l, r) => AreEqual(l, r),
        ['!', '='] => (l, r) => !AreEqual(l, r),
        // Comparison
        ['>'] => (l, r) => Compare(l, r) > 0,
        ['<'] => (l, r) => Compare(l, r) < 0,
        ['>', '='] => (l, r) => Compare(l, r) >= 0,
        ['<', '='] => (l, r) => Compare(l, r) <= 0,
            _ => throw new FormatException($"Operator '{Operator}' is not valid"),
        };
    }
}
