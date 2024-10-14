using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("true", HelpText = "Create a bool value set to 'true'")]
public sealed class True : IProducerCommand
{
    public ShellObject Execute(ShellContext context) => ShellScalar.True;
}
