using System.Text.Json;
using System.Text.Json.Serialization;

namespace CShell.DataModel.Json;

internal sealed class ShellScalarJsonConverter : JsonConverter<ShellScalar>
{
    public override ShellScalar? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ShellScalar(reader.GetString() ?? string.Empty),
            JsonTokenType.Number => new ShellScalar(GetNumber(ref reader)),
            JsonTokenType.True => ShellScalar.True,
            JsonTokenType.False => ShellScalar.False,
            JsonTokenType.Null => ShellScalar.Null,
            _ => throw new JsonException($"Unexpected {nameof(JsonTokenType)} '{reader.TokenType}' for {nameof(ShellScalar)}"),
        };

        static object GetNumber(ref Utf8JsonReader reader)
        {
            var @double = reader.GetDouble();
            if (double.IsInteger(@double))
            {
                return (long)@double;
            }
            return @double;
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
