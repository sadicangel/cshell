using CommandLine;
using static CShell.Commands.Operations;

namespace CShell.Commands;

[Verb("sort-by", HelpText = "Sort by the given columns, in increasing order.")]
public sealed class SortBy : IPipeCommand
{
    [Value(0, HelpText = "Operand", Required = true)]
    public required string Operand { get; init; }

    [Option('r', "reverse", HelpText = "Reverse order", Required = false, Default = false)]
    public bool Reverse { get; init; }

    public IEnumerable<Record> Execute(ShellContext context, IEnumerable<Record> records)
    {
        var comparer = !Reverse
            ? Comparer<object?>.Create(Compare)
            : Comparer<object?>.Create((l, r) => Compare(r, l));

        return records.OrderBy(r => r.GetValueOrDefault(Operand), comparer);
    }
}