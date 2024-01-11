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

    public ShellObject Execute(ShellContext context, ShellObject @object)
    {
        if (@object is not ShellArray array || array is not [ShellRecord first, ..])
            return @object;

        var comparer = !Reverse
            ? Comparer<object?>.Create(Compare)
            : Comparer<object?>.Create((l, r) => Compare(r, l));

        return new ShellArray(array.AsEnumerable().Cast<ShellRecord>().OrderBy(r => r.GetValueOrDefault(Operand).GetScalarValueOrDefault(), comparer));
    }
}