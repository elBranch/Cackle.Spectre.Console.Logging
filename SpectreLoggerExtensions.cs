using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cackle.Spectre.Console.Logging;

/// <summary>
///     Provides extension methods for configuring Spectre.Console logging.
/// </summary>
public static class SpectreLoggerExtensions
{
    /// <summary>
    ///     Adds a Spectre.Console logger to the logging builder.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder" /> to configure.</param>
    /// <returns>The <see cref="ILoggingBuilder" /> so that additional calls can be chained.</returns>
    public static ILoggingBuilder AddSpectreConsole(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton(new SpectreConsoleLoggerProvider());
        return builder;
    }
}