using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

public enum ScalarType
{
    i32, i64, str, date
}

[Verb("array", HelpText = "Create a 32-bit signed integer.")]
public sealed class ArrayS : IProducerCommand
{
    [Value(0, HelpText = "The type of values in the array.", Required = true)]
    public required ScalarType Type { get; init; }

    [Value(1, HelpText = "The values in the array")]
    public required IEnumerable<string> Values { get; init; }

    public ShellObject Execute(ShellContext context)
    {
        var type = Type switch
        {
            ScalarType.i32 => typeof(int),
            ScalarType.i64 => typeof(long),
            ScalarType.str => typeof(string),
            ScalarType.date => typeof(DateTime),
            _ => throw new InvalidOperationException($"Invalid type {Type}"),
        };

        return new ShellArray(Values.Select(v => new ShellScalar(Convert.ChangeType(v, type))));
    }
}
