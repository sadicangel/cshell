﻿using System.Diagnostics;

namespace CShell.DataModel;

internal static class ShellExtensions
{
    public static string ToStringSafe(this ShellScalar? obj) => obj?.Value?.ToString() ?? string.Empty;
    public static string ToStringSafe(this ShellArray? obj) => obj is null ? "null" : $"Array ({obj.Count} elements)";
    public static string ToStringSafe(this ShellRecord? obj) => obj is null ? "null" : $"Record ({obj.Count} properties)";
    public static string ToStringSafe(this ShellObject? obj) => obj switch
    {
        ShellScalar o => ToStringSafe(o),
        ShellArray o => ToStringSafe(o),
        ShellRecord o => ToStringSafe(o),
        null => "null",
        _ => throw new UnreachableException()
    };

    public static object? GetScalarValueOrDefault(this ShellObject obj) => (obj as ShellScalar)?.Value;

    public static ShellObject EvaluateExpression(this ShellObject obj, string expr)
    {
        if (expr is [] or "$")
            return obj;

        return obj.Switch(
            scalar => ShellScalar.Null,
            record => HandleRecord(record, expr),
            array => HandleArray(array, expr));

        static ShellObject HandleRecord(ShellRecord record, ReadOnlySpan<char> expr)
        {
            if (expr is not ['$', '.', ..])
                return ShellScalar.Null;

            expr = expr[2..];
            var end = expr.IndexOf('.');
            if (end < 0) end = expr.Length;

            return EvaluateExpression(record[expr[..end].ToString()] ?? ShellScalar.Null, expr[end..].ToString());
        }

        static ShellObject HandleArray(ShellArray array, ReadOnlySpan<char> expr)
        {
            if (expr is not ['$', '[', ..])
                return ShellScalar.Null;

            expr = expr[2..];
            var end = expr.IndexOf(']');
            if (end < 0) throw new InvalidOperationException("Missing ']'");
            if (!int.TryParse(expr[..end], out var index))
                return ShellScalar.Null;

            return EvaluateExpression(array[index], expr[end..].ToString());
        }
    }

    public static T Switch<T>(
        this ShellObject? obj,
        Func<ShellScalar, T> scalar,
        Func<ShellRecord, T> record,
        Func<ShellArray, T> array)
    {
        return obj switch
        {
            ShellScalar o => scalar(o),
            ShellArray o => array(o),
            ShellRecord o => record(o),
            _ => throw new UnreachableException()
        };
    }

    public static T Switch<T>(
        this IEnumerable<ShellObject> objects,
        Func<IEnumerable<ShellScalar>, T> scalars,
        Func<IEnumerable<ShellRecord>, T> records,
        Func<IEnumerable<ShellArray>, T> arrays,
        Func<T> empty)
    {
        return objects.FirstOrDefault() switch
        {
            ShellScalar => scalars(objects.Cast<ShellScalar>()),
            ShellArray => arrays(objects.Cast<ShellArray>()),
            ShellRecord => records(objects.Cast<ShellRecord>()),
            _ => empty()
        };
    }
}
