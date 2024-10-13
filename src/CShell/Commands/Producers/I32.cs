using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("i32", HelpText = "Create a 32-bit signed integer.")]
public sealed class I32 : IProducerCommand
{
    [Value(0, HelpText = "The value of the 32-bit signed integer.", Required = true)]
    public required int Value { get; init; }

    public ShellObject Execute(ShellContext context) => new ShellScalar(Value);
}
