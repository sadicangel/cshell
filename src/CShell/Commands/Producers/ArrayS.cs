using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("array", HelpText = "Create an array of scalar types.")]
file sealed class ArrayS : IProducerCommand
{
    [Value(0, HelpText = "The type of values in the array.", Required = true)]
    public required ScalarType Type { get; init; }

    [Value(1, HelpText = "The values in the array")]
    public required IEnumerable<string> Values { get; init; }

    public ShellObject Execute(ShellContext context)
    {
        var type = Type switch
        {
            ScalarType.Bool => typeof(bool),
            ScalarType.Int => typeof(long),
            ScalarType.Real => typeof(double),
            ScalarType.Str => typeof(string),
            ScalarType.Date => typeof(DateTime),
            _ => throw new InvalidOperationException($"Invalid type {Type}"),
        };

        return new ShellArray(Values.Select(v => new ShellScalar(Convert.ChangeType(v, type))));
    }
}
