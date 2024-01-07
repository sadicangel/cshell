namespace CShell.Commands;

internal static class Operations
{
    public static bool AreEqual(object? l, object? r)
    {
        if (l is null)
            return r is null;
        if (r is null)
            return false;

        return l.Equals(r);
    }

    public static int Compare(object? l, object? r)
    {
        if (l is null)
            return r is null ? 0 : -1;
        if (r is null)
            return 1;
        if (l is not IComparable c)
            throw new FormatException($"Value '{l}' is not comparable");
        return c.CompareTo(Convert.ChangeType(r, l.GetType()));
    }
}
