using System.Text.Json;
using System.Text.Json.Serialization;

namespace CShell.DataModel;

[JsonConverter(typeof(ShellScalarJsonConverter))]
public sealed record class ShellScalar(object? Value) : ShellObject
{
    public ShellScalar() : this(Value: null) { }

}

internal sealed class ShellScalarJsonConverter : JsonConverter<ShellScalar>
{
    public override ShellScalar? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new ShellScalar(reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => GetNumber(ref reader),
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Null => null,
            _ => throw new NotImplementedException(),
        });

        static object? GetNumber(ref Utf8JsonReader reader)
        {
            if (reader.TryGetInt32(out var i32)) return i32;
            if (reader.TryGetInt64(out var i64)) return i64;
            return reader.GetDouble();
        }
    }

    public override void Write(Utf8JsonWriter writer, ShellScalar value, JsonSerializerOptions options)
    {
        if (value.Value is null)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value.Value, options.GetTypeInfo(value.Value.GetType()));
    }
}
