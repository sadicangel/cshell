using CommandLine;
using CShell.DataModel;

namespace CShell.Commands.Pipes;

[Verb("where", HelpText = "Filter values based on a row condition.")]
public sealed class Where : Predicate, IPipeCommand
{
    protected override ShellObject Execute(ShellContext context, ShellObject @object, Func<object?, object?, bool> predicate)
    {
        if (@object is not ShellArray array || array is not [ShellRecord first, ..])
            return @object;

        return new ShellArray(array.AsEnumerable().Cast<ShellRecord>().Where(r => predicate(r.GetValueOrDefault(Left).GetScalarValueOrDefault(), Right)));
    }
}
