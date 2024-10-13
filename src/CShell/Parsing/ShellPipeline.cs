using CShell.Commands;

namespace CShell.Parsing;

internal readonly record struct ShellPipeline(
    IProducerCommand Producer,
    IReadOnlyList<IPipeCommand> Pipes,
    IConsumerCommand Consumer)
{
    public void Run(ShellContext context)
    {
        var @object = Producer.Execute(context);

        foreach (var pipe in Pipes)
            @object = pipe.Execute(context, @object);

        Consumer.Execute(context, @object);
    }
}
