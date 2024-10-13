using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

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
        if (@object is not ShellArray array)
            return @object;

        var comparer = !Reverse ? Operations.AscendingComparer : Operations.DescendingComparer;

        return new ShellArray(array.AsEnumerable().OrderBy(obj => obj.Evaluate(Operand).ValueUnsafe, comparer));
    }
}
