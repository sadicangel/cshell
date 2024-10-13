namespace CShell.Commands;

public sealed class CommandException(string command, string message) : Exception(message)
{
    public string Command { get; } = command;
}
