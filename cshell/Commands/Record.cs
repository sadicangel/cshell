using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CShell.Commands;

public sealed record Record() : IReadOnlyDictionary<string, object?>
{
    private readonly Dictionary<string, object?> _attributes = new(StringComparer.InvariantCultureIgnoreCase);

    public Record(IDictionary<string, object?> attributes) : this()
    {
        _attributes.EnsureCapacity(attributes.Count);
        foreach (var (key, value) in attributes)
            _attributes.Add(key, value);
    }

    public object? this[string key] { get => _attributes[key]; }

    public IEnumerable<string> Keys { get => _attributes.Keys; }
    public IEnumerable<object?> Values { get => _attributes.Values; }
    public int Count { get => _attributes.Count; }

    public bool ContainsKey(string key) => _attributes.ContainsKey(key);
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _attributes.GetEnumerator();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value) => _attributes.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_attributes).GetEnumerator();
}
