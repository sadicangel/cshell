using CShell.DataModel;

namespace CShell.Commands.Pipes;

public sealed class All : Predicate, IPipeCommand
{
    protected override ShellObject Execute(ShellContext context, ShellObject @object, Func<object?, object?, bool> predicate)
    {
        if (@object is not ShellArray array || array is not [ShellRecord first, ..])
            return @object;

        return new ShellScalar(array.AsEnumerable().Cast<ShellRecord>().All(r => predicate(r.GetValueOrDefault(Left).GetScalarValueOrDefault(), Right)));
    }
}
