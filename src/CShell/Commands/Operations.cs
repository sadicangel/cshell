using CShell.DataModel;

namespace CShell.Commands;

internal static class Operations
{
    public static readonly IComparer<object?> AscendingComparer = Comparer<object?>.Create(Compare);
    public static readonly IComparer<object?> DescendingComparer = Comparer<object?>.Create((l, r) => Compare(r, l));

    public static readonly Func<ShellObject, bool> IncludeAll = _ => true;

    private static int Compare(object? l, object? r)
    {
        if (l is null)
            return r is null ? 0 : -1;
        if (r is null)
            return 1;
        if (l is not IComparable c)
            throw new FormatException($"Value '{l}' is not comparable");
        return c.CompareTo(r);
    }

    private static bool AreEqual(object? l, object? r)
    {
        if (l is null)
            return r is null;
        if (r is null)
            return false;

        return l.Equals(r);
    }

    public static Func<ShellObject, bool> CreatePredicate(string left, string @operator, string right)
    {
        var operation = GetOperation(@operator);

        if (left.StartsWith('$'))
        {
            if (right.StartsWith('$'))
            {
                return obj => operation(obj.EvaluateExpression(left).GetScalarValueOrDefault(), obj.EvaluateExpression(right).GetScalarValueOrDefault());
            }

            return obj =>
            {
                var l = obj.EvaluateExpression(left).GetScalarValueOrDefault();
                var r = l is not null ? Convert.ChangeType(right, l.GetType()) : null;
                return operation(l, r);
            };
        }
        else if (right.StartsWith('$'))
        {
            return obj =>
            {
                var r = obj.EvaluateExpression(right).GetScalarValueOrDefault();
                var l = r is not null ? Convert.ChangeType(left, r.GetType()) : null;
                return operation(l, r);
            };

        }
        else
        {
            return obj => operation(left, right);
        }

        static Func<object?, object?, bool> GetOperation(ReadOnlySpan<char> @operator)
        {
            return @operator switch
            {
            // Equality
            ['=', '='] => (l, r) => AreEqual(l, r),
            ['!', '='] => (l, r) => !AreEqual(l, r),
            // Comparison
            ['>'] => (l, r) => Compare(l, r) > 0,
            ['<'] => (l, r) => Compare(l, r) < 0,
            ['>', '='] => (l, r) => Compare(l, r) >= 0,
            ['<', '='] => (l, r) => Compare(l, r) <= 0,
                _ => throw new FormatException($"Operator '{@operator}' is not valid"),
            };
        }
    }
}
