namespace CShell.DataModel;

public sealed record class ShellScalar(object? Value) : ShellObject
{
    public ShellScalar() : this(Value: null) { }

}
