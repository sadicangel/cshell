using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CShell.DataModel;

public sealed record ShellRecord() : ShellObject, IReadOnlyDictionary<string, ShellObject>
{
    private readonly Dictionary<string, ShellObject> _attributes = new(StringComparer.InvariantCultureIgnoreCase);

    public ShellRecord(IEnumerable<KeyValuePair<string, ShellObject>> attributes) : this()
    {
        if (attributes.TryGetNonEnumeratedCount(out var count))
            _attributes.EnsureCapacity(count);
        foreach (var (key, value) in attributes)
            _attributes.Add(key, value);
    }

    public override object? ValueUnsafe { get => null; }


    public ShellObject this[string key] { get => _attributes[key]; }

    public override string ToString() => $"{{ {string.Join("; ", _attributes.Select(e => $"{e.Key} = {e.Value}"))} }}";
    public override ShellObject Evaluate(ReadOnlySpan<char> expression)
    {
        if (expression is [] or "$")
            return this;

        if (expression is not ['$', '.', .. var rest])
            return ShellScalar.Null;

        var end = rest.IndexOf('.') is var i and >= 0 ? i : rest.Length;

        if (!_attributes.TryGetValue(rest[..end].ToString(), out var attribute))
            return ShellScalar.Null;

        return attribute.Evaluate(rest[end..]);
    }

    public IEnumerable<string> Keys { get => _attributes.Keys; }
    public IEnumerable<ShellObject> Values { get => _attributes.Values; }
    public int Count { get => _attributes.Count; }

    public bool ContainsKey(string key) => _attributes.ContainsKey(key);
    public IEnumerator<KeyValuePair<string, ShellObject>> GetEnumerator() => _attributes.GetEnumerator();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ShellObject value) => _attributes.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_attributes).GetEnumerator();
}
