using CommandLine;

namespace CShell.Commands.Producers;

[Verb("ls", HelpText = "List the filenames, sizes, and modification times of items in a directory.")]
public sealed class Ls : IProducerCommand
{
    public IEnumerable<Record> Execute(ShellContext context) => new DirectoryInfo(context.CurrentDirectory)
        .EnumerateFileSystemInfos()
        .Select(e => new Record(new Dictionary<string, object?>
        {
            ["Name"] = e.Name,
            ["Type"] = e is FileInfo ? "file" : "dir",
            ["FullName"] = e.FullName,
            ["CreationTime"] = e.CreationTimeUtc,
            ["LastWriteTime"] = e.LastWriteTimeUtc,
            ["Size"] = e is FileInfo f ? f.Length : default(long?)
        }));
}
