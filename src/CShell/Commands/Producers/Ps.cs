using System.Diagnostics;
using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("ps", HelpText = "View information about system processes.")]
public sealed class Ps : IProducerCommand
{
    public ShellObject Execute(ShellContext context) => new ShellArray(Process.GetProcesses()
        .Where(IsValidWindowsProcessId)
        .Select(p => new ShellRecord(new Dictionary<string, ShellObject>()
        {
            ["PID"] = new ShellScalar(p.Id),
            ["Name"] = new ShellScalar(p.ProcessName),
        })));

    private static bool IsValidWindowsProcessId(Process process) =>
        !OperatingSystem.IsWindows() || process.Id is not 0 and not 4;
}
