using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("utc", HelpText = "Create a UTC datetime.")]
public sealed class Date : IProducerCommand
{
    [Value(0, HelpText = "The value of the UTC datetime.", Required = true)]
    public required string Value { get; init; }

    public ShellObject Execute(ShellContext context) =>
        Value.Equals("now", StringComparison.OrdinalIgnoreCase)
            ? new ShellScalar(DateTime.UtcNow)
            : (ShellObject)new ShellScalar(Convert.ToDateTime(Value));
}
