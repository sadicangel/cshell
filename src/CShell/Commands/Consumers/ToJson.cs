﻿using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using CShell.DataModel;
using CShell.Parsing;
using Spectre.Console;
using Spectre.Console.Json;

namespace CShell.Commands.Consumers;

[Verb("json", HelpText = "Renders result as JSON.")]

public sealed class ToJson : IConsumerCommand
{
    public void Execute(ShellContext context, ShellObject @object)
    {
        var json = @object.Switch(
            s => JsonSerializer.Serialize(s, SourceGenerationContext.Default.ShellScalar),
            a => JsonSerializer.Serialize(a, SourceGenerationContext.Default.ShellArray),
            r => JsonSerializer.Serialize(r, SourceGenerationContext.Default.ShellRecord));
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
[JsonSerializable(typeof(ShellObject), GenerationMode = JsonSourceGenerationMode.Metadata)]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
