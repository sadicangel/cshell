using System.Collections;

namespace CShell.DataModel;

public sealed record class ShellArray() : ShellObject, IReadOnlyList<ShellObject>
{
    private readonly ShellObject[] _objects = [];

    public ShellArray(IEnumerable<ShellObject> objects) : this() => _objects = [.. objects];

    public override object? ValueUnsafe { get => null; }


    public ShellObject this[int index] => _objects[index];

    public int Length => _objects.Length;
    int IReadOnlyCollection<ShellObject>.Count => Length;

    public override string ToString() => $"[{string.Join(", ", _objects.Select(o => o.ToString()))}]";
    public override ShellObject Evaluate(ReadOnlySpan<char> expression)
    {
        if (expression is not ['$', '[', .. var rest])
            return ShellScalar.Null;

        var end = rest.IndexOf(']');
        if (end < 0) throw new InvalidOperationException("Missing ']'");
        if (!int.TryParse(rest[..end], out var index))
            return ShellScalar.Null;

        if (index < 0 || index >= _objects.Length)
            return ShellScalar.Null;

        var element = _objects[index];

        return element.Evaluate(rest[end..]);
    }

    public IEnumerator<ShellObject> GetEnumerator() => ((IEnumerable<ShellObject>)_objects).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
