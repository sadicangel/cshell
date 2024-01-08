using CommandLine;
using CShell.DataModel;
using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CShell.Commands.Consumers;

[Verb("json", HelpText = "Renders result as JSON.")]

public sealed class ToJson : IConsumerCommand
{
    public void Execute(ShellContext context, IEnumerable<ShellObject> objects)
    {
        var json = objects.FirstOrDefault() switch
        {
            ShellScalar => JsonSerializer.Serialize(objects.Cast<ShellScalar>(), SourceGenerationContext.Default.IEnumerableShellScalar),
            ShellArray => JsonSerializer.Serialize(objects.Cast<ShellArray>(), SourceGenerationContext.Default.IEnumerableShellArray),
            ShellRecord => JsonSerializer.Serialize(objects.Cast<ShellRecord>(), SourceGenerationContext.Default.IEnumerableShellRecord),
            _ => "{}",
        };
        context.Console.Write(new Panel(new JsonText(json)));
    }
}

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(byte))]
[JsonSerializable(typeof(sbyte))]
[JsonSerializable(typeof(short))]
[JsonSerializable(typeof(ushort))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(uint))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(ulong))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(DateOnly))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(TimeOnly))]
[JsonSerializable(typeof(IEnumerable<ShellScalar>))]
[JsonSerializable(typeof(IEnumerable<ShellArray>))]
[JsonSerializable(typeof(IEnumerable<ShellRecord>))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
