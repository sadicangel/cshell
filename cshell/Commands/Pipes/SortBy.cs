using CommandLine;
using CShell.DataModel;
using static CShell.Commands.Operations;

namespace CShell.Commands.Pipes;

[Verb("sort-by", HelpText = "Sort by the given columns, in increasing order.")]
public sealed class SortBy : IPipeCommand
{
    [Value(0, HelpText = "Operand", Required = true)]
    public required string Operand { get; init; }

    [Option('r', "reverse", HelpText = "Reverse order", Required = false, Default = false)]
    public bool Reverse { get; init; }

    public IEnumerable<ShellObject> Execute(ShellContext context, IEnumerable<ShellObject> objects)
    {
        if (objects.FirstOrDefault() is not ShellRecord first)
            return objects;

        var comparer = !Reverse
            ? Comparer<object?>.Create(Compare)
            : Comparer<object?>.Create((l, r) => Compare(r, l));

        return objects.Cast<ShellRecord>().OrderBy(r => r.GetValueOrDefault(Operand).GetScalarValueOrDefault(), comparer);
    }
}