namespace CShell.DataModel;

public sealed record class ShellScalar(object? Value) : ShellObject
{
    public ShellScalar() : this(Value: null) { }

    //public static implicit operator ShellScalar(byte? value) => new(value);
    //public static implicit operator ShellScalar(sbyte? value) => new(value);
    //public static implicit operator ShellScalar(short? value) => new(value);
    //public static implicit operator ShellScalar(ushort? value) => new(value);
    //public static implicit operator ShellScalar(int? value) => new(value);
    //public static implicit operator ShellScalar(uint? value) => new(value);
    //public static implicit operator ShellScalar(long? value) => new(value);
    //public static implicit operator ShellScalar(ulong? value) => new(value);
    //public static implicit operator ShellScalar(string? value) => new(value);
    //public static implicit operator ShellScalar(DateTime? value) => new(value);
    //public static implicit operator ShellScalar(DateTimeOffset? value) => new(value);
    //public static implicit operator ShellScalar(DateOnly? value) => new(value);
    //public static implicit operator ShellScalar(TimeSpan? value) => new(value);
    //public static implicit operator ShellScalar(TimeOnly? value) => new(value);
}
