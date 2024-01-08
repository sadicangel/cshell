using System.Diagnostics;

namespace CShell.DataModel;

internal static class ShellExtensions
{
    public static string ToStringSafe(this ShellScalar? obj) => obj?.Value?.ToString() ?? string.Empty;
    public static string ToStringSafe(this ShellArray? obj) => obj is null ? "null" : $"Array ({obj.Count} elements)";
    public static string ToStringSafe(this ShellRecord? obj) => obj is null ? "null" : $"Record ({obj.Count} properties)";
    public static string ToStringSafe(this ShellObject? obj) => obj switch
    {
        ShellScalar o => ToStringSafe(o),
        ShellArray o => ToStringSafe(o),
        ShellRecord o => ToStringSafe(o),
        null => "null",
        _ => throw new UnreachableException()
    };

    public static object? GetScalarValueOrDefault(this ShellObject? obj) => (obj as ShellScalar)?.Value;
}