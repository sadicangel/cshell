using System.Text.Json.Serialization;
using CShell.DataModel.Json;

namespace CShell.DataModel;

[JsonConverter(typeof(ShellScalarJsonConverter))]
public sealed record class ShellScalar : ShellObject
{
    public static readonly ShellScalar Null = new(new Null());
    public static readonly ShellScalar True = new(true);
    public static readonly ShellScalar False = new(false);

    public object Value { get; init; }

    public ShellScalar(object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    public override string ToString() => Value.ToString() ?? string.Empty;

    public override object? ValueUnsafe { get => Value is Null ? null : Value; }

    public override ShellObject Evaluate(ReadOnlySpan<char> expression) => expression is [] or "$" ? this : Null;
}

file readonly record struct Null { public override string ToString() => "null"; }
