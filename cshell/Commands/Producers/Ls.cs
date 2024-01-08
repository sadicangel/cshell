using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Producers;

[Verb("ls", HelpText = "List the filenames, sizes, and modification times of items in a directory.")]
public sealed class Ls : IProducerCommand
{
    public IEnumerable<ShellObject> Execute(ShellContext context) => new DirectoryInfo(context.CurrentDirectory)
        .EnumerateFileSystemInfos()
        .Select(e => new ShellRecord(new Dictionary<string, ShellScalar?>
        {
            ["Name"] = new(e.Name),
            ["Type"] = new(e is FileInfo ? "file" : "dir"),
            ["FullName"] = new(e.FullName),
            ["CreationTime"] = new(e.CreationTimeUtc),
            ["LastWriteTime"] = new(e.LastWriteTimeUtc),
            ["Size"] = new(e is FileInfo f ? f.Length : default(long?))
        }));
}
