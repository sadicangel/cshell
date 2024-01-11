using CShell;
using Spectre.Console;

var context = new ShellContext();

while (true)
{
    try
    {
        AnsiConsole.Markup("[green]cs> [/]");
        var input = Console.ReadLine();
        if (String.IsNullOrWhiteSpace(input))
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