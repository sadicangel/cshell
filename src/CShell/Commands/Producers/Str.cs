using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("str", HelpText = "Create a string.")]
public sealed class Str : IProducerCommand
{
    [Value(0, HelpText = "The value of the string.", Required = true)]
    public required string Value { get; init; }

    public ShellObject Execute(ShellContext context) => new ShellScalar(Value);
}
