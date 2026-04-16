namespace Cackle.Spectre.Console.Logging;

/// <summary>
///     Represents a null scope implementation that performs no operation when disposed. Used for logger scopes when scope
///     functionality is not required.
/// </summary>
internal sealed class NullScope : IDisposable
{
    private NullScope()
    {
    }

    /// <summary>
    ///     Gets the singleton instance of the null scope.
    /// </summary>
    internal static NullScope Instance { get; } = new();

    /// <summary>
    ///     Performs no operation. This is a null implementation of <see cref="IDisposable.Dispose" />.
    /// </summary>
    public void Dispose()
    {
    }
}