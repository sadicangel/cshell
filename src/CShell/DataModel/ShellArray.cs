using System.Collections;

namespace CShell.DataModel;

public sealed record class ShellArray() : ShellObject, IReadOnlyList<ShellObject>
{
    private readonly IReadOnlyList<ShellObject> _objects = [];

    public ShellArray(IEnumerable<ShellObject> objects) : this() => _objects = [.. objects];

    public ShellObject this[int index] { get => _objects[index]; }

    public int Count { get => _objects.Count; }

    public IEnumerator<ShellObject> GetEnumerator() => _objects.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
