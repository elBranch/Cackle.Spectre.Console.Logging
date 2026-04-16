using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Cackle.Spectre.Console.Logging;

/// <summary>
///     A logger implementation that outputs formatted log messages to the console using Spectre.Console. Provides
///     color-coded log levels and structured table output.
/// </summary>
/// <param name="categoryName">The category name for the logger, typically the full name of the class using the logger.</param>
public sealed class SpectreConsoleLogger(string categoryName) : ILogger
{
    private static readonly Lock Lock = new();

    /// <summary>
    ///     Begins a logical operation scope. This implementation returns a null scope as scoping is not supported.
    /// </summary>
    /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
    /// <param name="state">The identifier for the scope.</param>
    /// <returns>A <see cref="NullScope" /> that performs no operation when disposed.</returns>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return NullScope.Instance;
    }

    /// <summary>
    ///     Checks if the given log level is enabled.
    /// </summary>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns><c>true</c> if the log level is enabled; otherwise, <c>false</c>.</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <summary>
    ///     Writes a log entry to the console using Spectre.Console formatting. Outputs a color-coded table with timestamp, log
    ///     level, category, and message.
    /// </summary>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    /// <param name="logLevel">The severity level of the log entry.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="state">The entry to be written. Can be also an object.</param>
    /// <param name="exception">The exception related to this entry.</param>
    /// <param name="formatter">Function to create a string message from the state and exception.</param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var (levelText, levelColour, levelBackground) = logLevel switch
        {
            LogLevel.Trace => ("trc", "tan", string.Empty),
            LogLevel.Debug => ("dbg", "chartreuse3", string.Empty),
            LogLevel.Information => ("inf", "dodgerblue1", string.Empty),
            LogLevel.Warning => ("wrn", "darkorange", string.Empty),
            LogLevel.Error => ("err", "red3_1", string.Empty),
            LogLevel.Critical => ("ftl", "white", " on red3_1"),
            LogLevel.None => (string.Empty, string.Empty, string.Empty),
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        string message;
        if (state is IEnumerable<KeyValuePair<string, object?>> structure)
        {
            var list = structure.ToList();
            var template = list.FirstOrDefault(kv => kv.Key == "{OriginalFormat}").Value?.ToString() ??
                           formatter(state, exception);

            message = template;
            foreach (var kv in list)
            {
                if (kv.Key == "{OriginalFormat}") continue;

                var placeholder = $"{{{kv.Key}}}";
                var strValue = kv.Value?.ToString().EscapeMarkup() ?? "(null)";
                message = message.Replace(placeholder, $"[cyan]{strValue}[/]");
            }
        }
        else
        {
            message = formatter(state, exception).EscapeMarkup();
        }

        lock (Lock)
        {
            Table table = new();
            table.Border(TableBorder.None)
                .HideHeaders()
                .AddColumn("Time", c =>
                {
                    c.Width = 8;
                    c.NoWrap = true;
                })
                .AddColumn("Level", c =>
                {
                    c.Width = 5;
                    c.NoWrap = true;
                })
                .AddColumn("Category", c =>
                {
                    c.Width = 20;
                    c.NoWrap = true;
                    c.Alignment = Justify.Right;
                })
                .AddColumn("Message");

            table.AddRow($"[grey]{DateTime.Now:HH:mm:ss}[/]",
                $"[[[bold {levelColour}{levelBackground}]{levelText,-3}[/]]]",
                FormatCategory(categoryName),
                message);

            AnsiConsole.Write(table);

            if (exception != null)
                AnsiConsole.WriteException(exception);
        }
    }

    /// <summary>
    ///     Formats the category name to fit within the specified maximum width. Truncates long category names with an ellipsis
    ///     prefix.
    /// </summary>
    /// <param name="category">The category name to format.</param>
    /// <param name="maxWidth">The maximum width for the formatted category. Default is 20 characters.</param>
    /// <returns>A grey-colored, markup-escaped category string suitable for Spectre.Console output.</returns>
    private static string FormatCategory(string category, int maxWidth = 20)
    {
        if (category.Length > maxWidth) category = "…" + category[^(maxWidth - 1)..];
        return $"[grey]{category.EscapeMarkup()}[/]";
    }
}