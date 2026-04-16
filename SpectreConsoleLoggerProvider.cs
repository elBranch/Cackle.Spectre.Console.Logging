using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Cackle.Spectre.Console.Logging;

/// <summary>
///     Provides instances of <see cref="SpectreConsoleLogger" /> for different categories. Implements a singleton pattern
///     per category name to ensure consistent logger instances.
/// </summary>
public sealed class SpectreConsoleLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, SpectreConsoleLogger> _loggers = new();

    /// <summary>
    ///     Disposes the logger provider and clears all cached logger instances.
    /// </summary>
    public void Dispose()
    {
        _loggers.Clear();
    }

    /// <summary>
    ///     Creates or retrieves a logger instance for the specified category.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>A <see cref="SpectreConsoleLogger" /> instance for the specified category.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, new SpectreConsoleLogger(categoryName));
    }
}