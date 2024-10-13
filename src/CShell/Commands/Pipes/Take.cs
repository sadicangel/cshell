using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Pipes;

[Verb("take", HelpText = "Take a specified number of elements and discard the remaining.")]
public sealed class Take : IPipeCommand
{
    [Value(0, HelpText = "Count", Required = true)]
    public required int Count { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object)
    {
        if (@object is not ShellArray array)
            return @object;

        return new ShellArray(array.AsEnumerable().Take(Count));
    }
}