using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CShell.DataModel;

[JsonDerivedType(typeof(ShellScalar))]
[JsonDerivedType(typeof(ShellArray))]
[JsonDerivedType(typeof(ShellRecord))]
public abstract record class ShellObject
{
    public abstract override string ToString();

    public abstract ShellObject Evaluate(ReadOnlySpan<char> expression);

    public T Switch<T>(Func<ShellScalar, T> scalar, Func<ShellArray, T> array, Func<ShellRecord, T> record) => this switch
    {
        ShellScalar o => scalar(o),
        ShellArray o => array(o),
        ShellRecord o => record(o),
        _ => throw new UnreachableException($"Unexpected {nameof(ShellObject)} with type '{GetType()}'")
    };
}
