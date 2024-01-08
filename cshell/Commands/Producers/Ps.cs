using CommandLine;

namespace CShell.Commands.Producers;

[Verb("ps", HelpText = "View information about system processes.")]
public sealed class Ps : IProducerCommand
{
    public IEnumerable<Record> Execute(ShellContext context) => Enumerable.Empty<Record>();
}
