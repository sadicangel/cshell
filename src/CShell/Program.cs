using System.Globalization;
using CShell.Parsing;
using Spectre.Console;

// Fix dumb datetime format.
var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
Thread.CurrentThread.CurrentCulture = culture;

var context = new ShellContext();

while (true)
{
    try
    {
        AnsiConsole.Markup("[green]cs> [/]");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            continue;

        if (input is "exit")
            return 0;

        var pipeline = ShellParser.Parse(input);

        pipeline.Run(context);
    }
    catch (Exception ex)
    {
        if (!context.Verbose)
            AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message}[/]");
        else
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    }
}
