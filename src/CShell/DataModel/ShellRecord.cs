using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CShell.DataModel;

public sealed record ShellRecord() : ShellObject, IReadOnlyDictionary<string, ShellObject?>
{
    private readonly Dictionary<string, ShellObject?> _attributes = new(StringComparer.InvariantCultureIgnoreCase);

    public ShellRecord(IEnumerable<KeyValuePair<string, ShellScalar?>> attributes) : this()
    {
        if (attributes.TryGetNonEnumeratedCount(out var count))
            _attributes.EnsureCapacity(count);
        foreach (var (key, value) in attributes)
            _attributes.Add(key, value);
    }

    public ShellObject? this[string key] { get => _attributes[key]; }

    public IEnumerable<string> Keys { get => _attributes.Keys; }
    public IEnumerable<ShellObject?> Values { get => _attributes.Values; }
    public int Count { get => _attributes.Count; }

    public bool ContainsKey(string key) => _attributes.ContainsKey(key);
    public IEnumerator<KeyValuePair<string, ShellObject?>> GetEnumerator() => _attributes.GetEnumerator();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ShellObject? value) => _attributes.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_attributes).GetEnumerator();
}
