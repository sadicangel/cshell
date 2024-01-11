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

    public static T Switch<T>(
        this ShellObject? obj,
        Func<ShellScalar, T> Scalar,
        Func<ShellRecord, T> Record,
        Func<ShellArray, T> Array,
        Func<T> Null)
    {
        return obj switch
        {
            ShellScalar o => Scalar(o),
            ShellArray o => Array(o),
            ShellRecord o => Record(o),
            _ => Null()
        };
    }

    public static T Switch<T>(
        this IEnumerable<ShellObject> objects,
        Func<IEnumerable<ShellScalar>, T> Scalars,
        Func<IEnumerable<ShellRecord>, T> Records,
        Func<IEnumerable<ShellArray>, T> Arrays,
        Func<T> Empty)
    {
        return objects.FirstOrDefault() switch
        {
            ShellScalar => Scalars(objects.Cast<ShellScalar>()),
            ShellArray => Arrays(objects.Cast<ShellArray>()),
            ShellRecord => Records(objects.Cast<ShellRecord>()),
            _ => Empty()
        };
    }
}