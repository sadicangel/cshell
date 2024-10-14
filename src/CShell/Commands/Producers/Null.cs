using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("null", HelpText = "Create the 'null' value")]
public sealed class Null : IProducerCommand
{
    public ShellObject Execute(ShellContext context) => ShellScalar.False;
}
