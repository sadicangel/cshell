using Spectre.Console;

namespace CShell.Parsing;

public sealed class ShellContext
{
    public IAnsiConsole Console { get; } = AnsiConsole.Console;
    public string CurrentDirectory { get => Environment.CurrentDirectory; set => Environment.CurrentDirectory = value; }
    public bool Verbose { get; set; }
}