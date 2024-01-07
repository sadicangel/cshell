using Spectre.Console;

namespace CShell;

public sealed class ShellContext
{
    public required IAnsiConsole Console { get; init; }
    public required string CurrentDirectory { get; set; }
}