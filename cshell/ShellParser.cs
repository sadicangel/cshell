using CommandLine;
using CShell.Commands;
using CShell.Commands.Consumers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
    private static readonly Type[] ProducerTypes = FindCommands<IProducerCommand>();
    private static readonly Type[] PipeTypes = FindCommands<IPipeCommand>();
    private static readonly Type[] ConsumerTypes = FindCommands<IConsumerCommand>();

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private static Type[] FindCommands<T>() => [.. typeof(T).Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(T)))];

    public static CShellPipeline Parse(ReadOnlySpan<char> input)
    {
        var commandCount = input.Split(Ranges, '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var i = 0;
        var producer = ParseProducerCommand(input[Ranges[i++]]);

        var pipes = new List<IPipeCommand>(commandCount);
        for (; i < commandCount - 1; ++i)
            pipes.Add(ParsePipeCommand(input[Ranges[i]]));

        IConsumerCommand? consumer;
        if (TryParsePipeCommand(input[Ranges[i]], out var command, out _))
        {
            pipes.Add(command);
            consumer = new ToTable();
        }
        else if (i < commandCount)
        {
            consumer = ParseConsumerCommand(input[Ranges[i]]);
        }
        else
        {
            consumer = new ToTable();
        }

        return new CShellPipeline(producer, pipes, consumer);
    }

    private static IProducerCommand ParseProducerCommand(ReadOnlySpan<char> command)
    {
        var args = SplitArgs(command);
        var result = Parser.ParseArguments(args, ProducerTypes);
        return result.Tag is ParserResultType.Parsed ? (IProducerCommand)result.Value : throw new ShellParserException(result.Errors);
    }

    private static bool TryParsePipeCommand(ReadOnlySpan<char> command, [MaybeNullWhen(false)] out IPipeCommand pipeCommand, out IEnumerable<Error> errors)
    {
        var args = SplitArgs(command);
        pipeCommand = null;
        var result = Parser.ParseArguments(args, PipeTypes);
        errors = result.Errors;
        if (result.Tag is ParserResultType.Parsed)
        {
            pipeCommand = (IPipeCommand)result.Value;
            return true;
        }
        return false;
    }

    private static IPipeCommand ParsePipeCommand(ReadOnlySpan<char> command) =>
        TryParsePipeCommand(command, out var pipeCommand, out var errors) ? pipeCommand : throw new ShellParserException(errors);

    private static IConsumerCommand ParseConsumerCommand(ReadOnlySpan<char> command)
    {
        var args = SplitArgs(command);
        var result = Parser.ParseArguments(args, ConsumerTypes);
        return result.Tag is ParserResultType.Parsed ? (IConsumerCommand)result.Value : throw new ShellParserException(result.Errors);
    }

    private static List<string> SplitArgs(ReadOnlySpan<char> command)
    {
        var arg = new StringBuilder();
        var inDelimitedString = false;
        var args = new List<string>();
        foreach (var c in command)
        {
            switch (c)
            {
                case '"' when inDelimitedString && arg.Length > 0:
                    args.Add(arg.ToString());
                    arg.Clear();
                    inDelimitedString = !inDelimitedString;
                    break;

                case '"':
                    inDelimitedString = !inDelimitedString;
                    break;

                case ' ' when !inDelimitedString && arg.Length > 0:
                    args.Add(arg.ToString());
                    arg.Clear();
                    break;

                case ' ' when inDelimitedString:
                    arg.Append(c);
                    break;

                default:
                    arg.Append(c);
                    break;
            }
        }

        if (arg.Length > 0)
        {
            args.Add(arg.ToString());
            arg.Clear();
        }

        return args;
    }
}
