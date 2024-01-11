using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("i64", HelpText = "Create a 64-bit signed integer.")]
public sealed class I64 : IProducerCommand
{
    [Value(0, HelpText = "The value of the 64-bit signed integer.", Required = true)]
    public required long Value { get; init; }

    public ShellObject Execute(ShellContext context) => new ShellScalar(Value);
}
