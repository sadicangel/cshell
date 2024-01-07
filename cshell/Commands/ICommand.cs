namespace CShell.Commands;

interface IProducerCommand
{
    IEnumerable<Record> Execute(ShellContext context);
}

interface IConsumerCommand
{
    int Execute(ShellContext context, IEnumerable<Record> records);
}

interface IPipeCommand
{
    IEnumerable<Record> Execute(ShellContext context, IEnumerable<Record> records);
}
