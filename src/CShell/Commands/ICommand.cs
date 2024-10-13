using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands;

interface IProducerCommand
{
    ShellObject Execute(ShellContext context);
}

interface IConsumerCommand
{
    void Execute(ShellContext context, ShellObject @object);
}

interface IPipeCommand
{
    ShellObject Execute(ShellContext context, ShellObject @object);
}
