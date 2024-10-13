namespace CShell.DataModel;

internal static class ShellExtensions
{
    public static object? GetScalarValueOrDefault(this ShellObject obj) => (obj as ShellScalar)?.Value;
}
