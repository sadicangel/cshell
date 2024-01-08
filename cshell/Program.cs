using CShell;
using Spectre.Console;

var context = new ShellContext
{
    Console = AnsiConsole.Console,
    CurrentDirectory = Environment.CurrentDirectory,
};

while (true)
{
    try
    {
        AnsiConsole.Markup("[green]cs> [/]");
        var input = Console.ReadLine().AsSpan();

        if (input.Length == 0)
            continue;

        if (input == "exit")
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