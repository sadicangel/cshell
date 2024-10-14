using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("false", HelpText = "Create a bool value set to 'false'")]
public sealed class False : IProducerCommand
{
    public ShellObject Execute(ShellContext context) => ShellScalar.False;
}
