using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("ps", HelpText = "View information about system processes.")]
public sealed class Ps : IProducerCommand
{
    public IEnumerable<ShellObject> Execute(ShellContext context) => Enumerable.Empty<ShellObject>();
}
