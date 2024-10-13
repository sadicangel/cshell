using CommandLine;
using CShell.DataModel;
using CShell.Parsing;

namespace CShell.Commands.Producers;

[Verb("ls", HelpText = "List the filenames, sizes, and modification times of items in a directory.")]
public sealed class Ls : IProducerCommand
{
    private static readonly EnumerationOptions s_defaultEnumerationOptions = new();
    private static readonly EnumerationOptions s_showAllEnumerationOptions = new() { AttributesToSkip = default };

    [Option('a', "all", HelpText = "Show hidden and system files")]
    public bool All { get; init; }

    public ShellObject Execute(ShellContext context) => new ShellArray(new DirectoryInfo(context.CurrentDirectory)
        .EnumerateFileSystemInfos("*", All ? s_showAllEnumerationOptions : s_defaultEnumerationOptions)
        .Select(fs => new ShellRecord(fs switch
        {
            FileInfo file => new Dictionary<string, ShellObject>
            {
                ["Name"] = new ShellScalar(fs.Name),
                ["Type"] = new ShellScalar("file"),
                ["FullName"] = new ShellScalar(fs.FullName),
                ["CreationTime"] = new ShellScalar(fs.CreationTimeUtc),
                ["LastWriteTime"] = new ShellScalar(fs.LastWriteTimeUtc),
                ["Size"] = new ShellScalar(file.Length)
            },

            DirectoryInfo directory => new Dictionary<string, ShellObject>
            {
                ["Name"] = new ShellScalar(fs.Name),
                ["Type"] = new ShellScalar("dir"),
                ["FullName"] = new ShellScalar(fs.FullName),
                ["CreationTime"] = new ShellScalar(fs.CreationTimeUtc),
                ["LastWriteTime"] = new ShellScalar(fs.LastWriteTimeUtc),
            },

            _ => throw new NotSupportedException(fs.GetType().Name)
        })));
}
