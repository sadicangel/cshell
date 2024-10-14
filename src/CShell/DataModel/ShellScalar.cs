using System.Diagnostics;
using System.Text.Json.Serialization;
using CShell.DataModel.Json;

namespace CShell.DataModel;

[JsonConverter(typeof(ShellScalarJsonConverter))]
public sealed record class ShellScalar : ShellObject
{
    public static readonly ShellScalar Null = new(new Unit());
    public static readonly ShellScalar True = new(true);
    public static readonly ShellScalar False = new(false);

    public ScalarType Type => Value switch
    {
        Unit => ScalarType.Null,
        bool => ScalarType.Bool,
        long => ScalarType.Int,
        double => ScalarType.Real,
        string => ScalarType.Str,
        DateTime => ScalarType.Date,
        _ => throw new UnreachableException($"Unexpected {nameof(ScalarType)} for value of type {Value.GetType()}")
    };

    public object Value { get; init; }

    public ShellScalar(object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    public override string ToString() => Value.ToString() ?? string.Empty;

    public override object? ValueUnsafe { get => Value is Unit ? null : Value; }

    public override ShellObject Evaluate(ReadOnlySpan<char> expression) => expression is [] or "$" ? this : Null;
}

file readonly record struct Unit { public override string ToString() => "null"; }

public enum ScalarType
{
    Null,
    Bool,
    Int,
    Real,
    Str,
    Date,
}
