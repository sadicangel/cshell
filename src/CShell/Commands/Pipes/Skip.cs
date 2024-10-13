using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Pipes;

[Verb("skip", HelpText = "Skip a specified number of elements and return the remaining.")]
public sealed class Skip : IPipeCommand
{
    [Value(0, HelpText = "Count", Required = true)]
    public required int Count { get; init; }

    public ShellObject Execute(ShellContext context, ShellObject @object)
    {
        if (@object is not ShellArray array)
            return @object;

        return new ShellArray(array.AsEnumerable().Skip(Count));
    }
}
