using CommandLine;
using CShell.Commands;
using System.Diagnostics.CodeAnalysis;

namespace CShell;

internal readonly record struct CShellPipeline(
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

internal static class ShellParser
{
    private static readonly Parser Parser = new(s => s.HelpWriter = null);
    private static readonly Range[] Ranges = new Range[256];

    public static CShellPipeline Parse(ReadOnlySpan<char> input)
    {
        var commandCount = input.Split(Ranges, '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var i = 0;
        var producer = ParseProducerCommand(input[Ranges[i++]]);

        var pipes = new List<IPipeCommand>(commandCount);
        for (; i < commandCount - 1; ++i)
            pipes.Add(ParseFilterCommand(input[Ranges[i]]));

        IConsumerCommand? consumer;
        if (!TryParseFilterCommand(input[Ranges[i]], out var command))
        {
            consumer = i > 1
                ? ParseConsumerCommand(input[Ranges[i]])
                : new ToTable();
        }
        else
        {
            pipes.Add(command);
            consumer = new ToTable();
        }

        return new CShellPipeline(producer, pipes, consumer);
    }

    private static IProducerCommand ParseProducerCommand(ReadOnlySpan<char> command)
    {
        var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return Parser.ParseArguments<Ls, Ps>(args)
            .MapResult<Ls, Ps, IProducerCommand>(ls => ls, ps => ps, e => throw new ShellParserException(e));
    }

    private static bool TryParseFilterCommand(ReadOnlySpan<char> command, [MaybeNullWhen(false)] out IPipeCommand filterCommand)
    {
        var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var result = default(IPipeCommand);
        Parser
            .ParseArguments<Where, SortBy>(args)
            .WithParsed(f => result = (IPipeCommand)f)
            .WithNotParsed(_ => result = null);
        filterCommand = result;
        return filterCommand is not null;
    }

    private static IPipeCommand ParseFilterCommand(ReadOnlySpan<char> command)
    {
        var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return Parser.ParseArguments<Where, SortBy>(args)
            .MapResult<Where, SortBy, IPipeCommand>(where => where, sortBy => sortBy, e => throw new ShellParserException(e));
    }

    private static IConsumerCommand ParseConsumerCommand(ReadOnlySpan<char> command)
    {
        var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return Parser.Default.ParseArguments<ToTable, object>(args)
            .MapResult<ToTable, object, IConsumerCommand>(table => table, obj => throw new InvalidOperationException("Command not configured"), e => throw new ShellParserException(e));
    }
}
