using CommandLine;
using CShell;
using CShell.Commands;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

var ranges = new Range[256];

var context = new ShellContext
{
    Console = AnsiConsole.Console,
    CurrentDirectory = Environment.CurrentDirectory,
};

while (true)
{
    var input = AnsiConsole.Ask<string>("[green]cs>[/]").AsSpan();

    var commandCount = input.Split(ranges, '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    var i = 0;
    var producer = ParseProducerCommand(input[ranges[i++]]);

    var commands = new List<IPipeCommand>(commandCount);
    for (; i < commandCount - 1; ++i)
        commands.Add(ParseFilterCommand(input[ranges[i]]));

    var consumer = default(IConsumerCommand)!;

    if (!TryParseFilterCommand(input[ranges[i]], out var command))
    {
        consumer = ParseConsumerCommand(input[ranges[i]]);
    }
    else
    {
        commands.Add(command);
        consumer = new ToTable();
    }

    commands.Aggregate(producer.Execute(context), (rec, cmd) => cmd.Execute(context, rec), rec => consumer.Execute(context, rec));
}

static IProducerCommand ParseProducerCommand(ReadOnlySpan<char> command)
{
    var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return Parser.Default.ParseArguments<Ls, Ps>(args)
        .MapResult<Ls, Ps, IProducerCommand>(ls => ls, ps => ps, e => throw new InvalidOperationException());
}

static bool TryParseFilterCommand(ReadOnlySpan<char> command, [MaybeNullWhen(false)] out IPipeCommand filterCommand)
{
    var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var result = default(IPipeCommand);
    new Parser(s => s.HelpWriter = null)
        .ParseArguments<Where, SortBy>(args)
        .WithParsed(f => result = (IPipeCommand)f)
        .WithNotParsed(_ => result = null);
    filterCommand = result;
    return filterCommand is not null;
}

static IPipeCommand ParseFilterCommand(ReadOnlySpan<char> command)
{
    var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return Parser.Default.ParseArguments<Where, SortBy>(args)
        .MapResult<Where, SortBy, IPipeCommand>(where => where, sortBy => sortBy, e => throw new InvalidOperationException());
}

static IConsumerCommand ParseConsumerCommand(ReadOnlySpan<char> command)
{
    var args = command.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return Parser.Default.ParseArguments<ToTable, object>(args)
        .MapResult<ToTable, object, IConsumerCommand>(table => table, obj => throw new InvalidOperationException(), e => throw new InvalidOperationException());
}
