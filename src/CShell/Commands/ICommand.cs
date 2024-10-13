using CShell.DataModel;

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
