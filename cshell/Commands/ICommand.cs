using CShell.DataModel;

namespace CShell.Commands;

interface IProducerCommand
{
    IEnumerable<ShellObject> Execute(ShellContext context);
}

interface IConsumerCommand
{
    void Execute(ShellContext context, IEnumerable<ShellObject> objects);
}

interface IPipeCommand
{
    IEnumerable<ShellObject> Execute(ShellContext context, IEnumerable<ShellObject> objects);
}
