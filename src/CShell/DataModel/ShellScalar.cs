using System.Text.Json.Serialization;
using CShell.DataModel.Json;

namespace CShell.DataModel;

[JsonConverter(typeof(ShellScalarJsonConverter))]
public sealed record class ShellScalar(object Value) : ShellObject
{
    public static readonly ShellScalar Null = new(new NullValue());
    public static readonly ShellScalar True = new(true);
    public static readonly ShellScalar False = new(false);

    public override string ToString() => Value.ToString() ?? string.Empty;

    public override ShellObject Evaluate(ReadOnlySpan<char> expression) => expression is [] or "$" ? this : Null;
}

file readonly record struct NullValue { public override string ToString() => "null"; }
