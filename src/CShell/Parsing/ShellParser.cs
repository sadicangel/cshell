using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommandLine;
using CShell.Commands;
using CShell.Commands.Consumers;

namespace CShell.Parsing;

internal static class ShellParser
{
    private static readonly Parser s_parser = new(opts =>
    {
        opts.HelpWriter = null;
        opts.CaseSensitive = false;
    });
    private static readonly Range[] s_ranges = new Range[256];
    private static readonly Type[] s_producerTypes = FindCommands<IProducerCommand>();
    private static readonly Type[] s_pipeTypes = FindCommands<IPipeCommand>();
    private static readonly Type[] s_consumerTypes = FindCommands<IConsumerCommand>();

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private static Type[] FindCommands<T>() => [.. typeof(T).Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(T)))];

    public static ShellPipeline Parse(ReadOnlySpan<char> input)
    {
        var commandCount = input.Split(s_ranges, '|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var i = 0;
        var producer = ParseProducerCommand(input[s_ranges[i++]]);

        var pipes = new List<IPipeCommand>(commandCount);
        for (; i < commandCount - 1; ++i)
            pipes.Add(ParsePipeCommand(input[s_ranges[i]]));

        IConsumerCommand? consumer;
        if (i < commandCount)
        {
            if (TryParsePipeCommand(input[s_ranges[i]], out var command, out _))
            {
                pipes.Add(command);
                consumer = new ToTable();
            }
            else
            {
                consumer = ParseConsumerCommand(input[s_ranges[i]]);
            }
        }
        else
        {
            consumer = new ToTable();
        }

        return new ShellPipeline(producer, pipes, consumer);
    }

    private static IProducerCommand ParseProducerCommand(ReadOnlySpan<char> command)
    {
        var args = SplitArgs(command);
        var result = s_parser.ParseArguments(args, s_producerTypes);
        return result.Tag is ParserResultType.Parsed ? (IProducerCommand)result.Value : throw new ShellParserException(result.Errors);
    }

    private static bool TryParsePipeCommand(ReadOnlySpan<char> command, [MaybeNullWhen(false)] out IPipeCommand pipeCommand, out IEnumerable<Error> errors)
    {
        var args = SplitArgs(command);
        pipeCommand = null;
        var result = s_parser.ParseArguments(args, s_pipeTypes);
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
        var result = s_parser.ParseArguments(args, s_consumerTypes);
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
