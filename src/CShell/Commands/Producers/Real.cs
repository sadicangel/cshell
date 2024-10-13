using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("real", HelpText = "Create a 64-bit IEEE double-precision floating-point number.")]
public sealed class Real : IProducerCommand
{
    [Value(0, HelpText = "The value of the 64-bit IEEE double-precision floating-point number.", Required = true)]
    public required double Value { get; init; }

    public ShellObject Execute(ShellContext context) => new ShellScalar(Value);
}
