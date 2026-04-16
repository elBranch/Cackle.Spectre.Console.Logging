# Cackle.Spectre.Console.Logging

A beautiful, color-coded console logging provider for .NET that integrates [Spectre.Console](https://spectreconsole.net/) with Microsoft.Extensions.Logging.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![Spectre.Console](https://img.shields.io/badge/Spectre.Console-0.55.0-5C2D91)](https://spectreconsole.net/)

## Features

- 🎨 **Beautiful Color-Coded Output** - Each log level has its own distinct color scheme
- 📋 **Structured Table Layout** - Organized columns for timestamp, level, category, and message
- 🔍 **Template Parameter Highlighting** - Structured logging parameters are highlighted in cyan
- 💥 **Rich Exception Formatting** - Exceptions are displayed with Spectre.Console's pretty-printed format
- ⚡ **Thread-Safe** - Uses locking to ensure proper console output in multi-threaded scenarios
- 🪶 **Lightweight** - Minimal dependencies and overhead

## Installation

```bash
dotnet add package Cackle.Spectre.Console.Logging
```

## Quick Start

```csharp
using Cackle.Spectre.Console.Logging;
using Microsoft.Extensions.Logging;

// Add to your logging configuration
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSpectreConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Application started");
logger.LogWarning("This is a warning message");
logger.LogError("An error occurred");
```

### With Dependency Injection

```csharp
using Cackle.Spectre.Console.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSpectreConsole();

var app = builder.Build();
await app.RunAsync();
```

## Log Levels and Colors

The logger supports all standard .NET log levels with distinct color schemes:

| Level | Abbreviation | Color | Background |
|-------|--------------|-------|------------|
| Trace | `trc` | Tan | - |
| Debug | `dbg` | Chartreuse | - |
| Information | `inf` | Dodger Blue | - |
| Warning | `wrn` | Dark Orange | - |
| Error | `err` | Red | - |
| Critical | `ftl` | White | Red |

## Output Format

Log entries are displayed in a table format with the following columns:

- **Time** - HH:mm:ss format (grey)
- **Level** - 3-character abbreviation with color coding
- **Category** - Logger category name, right-aligned (grey)
- **Message** - The log message with highlighted parameters

Example output:
```
12:34:56 [inf] MyApp.Services Application started
12:34:57 [wrn]     MyApp.Api Request timeout after 30 seconds
12:34:58 [err] MyApp.Database Connection failed to localhost:5432
```

## Structured Logging

The logger supports structured logging with automatic parameter highlighting:

```csharp
logger.LogInformation("User {UserId} logged in from {IpAddress}", 123, "192.168.1.1");
// Output: User 123 logged in from 192.168.1.1
//         (with 123 and 192.168.1.1 highlighted in cyan)
```

## Exception Handling

Exceptions are automatically formatted using Spectre.Console's rich exception display:

```csharp
try
{
    throw new InvalidOperationException("Something went wrong!");
}
catch (Exception ex)
{
    logger.LogError(ex, "Operation failed");
}
```

This will display the exception with:
- Full exception type and message
- Stack trace with syntax highlighting
- Inner exceptions (if any)

## Performance Considerations

- Uses `Lock` (C# 13+) for thread-safe console access
- Logger instances are cached per category name
- Markup escaping is applied to prevent injection issues
- Long category names are truncated with ellipsis (max 20 characters)

## Requirements

- .NET 10.0 or later
- Microsoft.Extensions.Logging.Abstractions 10.0.6+
- Spectre.Console 0.55.0+

## License

[MIT License](LICENSE)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Related Projects

- [Spectre.Console](https://spectreconsole.net/) - A .NET library that makes it easier to create beautiful console applications
- [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) - Logging infrastructure for .NET
