using CShell.Commands;

namespace CShell.Parsing;

internal readonly record struct ShellPipeline(
    IProducerCommand Producer,
    IReadOnlyList<IPipeCommand> Pipes,
    IConsumerCommand Consumer)
{
    public void Run(ShellContext context)
    {
        var records = Producer.Execute(context);

        foreach (var pipe in Pipes)
            records = pipe.Execute(context, records);

        Consumer.Execute(context, records);
    }
}
