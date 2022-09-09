using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging;


namespace DriverLicenseApplication.Adapters.Tests.Spies;

public class LoggerSpy<T> : ILogger<T>
{
    private readonly List<(LogLevel, string, Type?)> logs = new();
    
    public void AssertThatHaveLogged(LogLevel level, string message, Type? exceptionType = null)
    {
        this.logs.Should().Contain((level, message, exceptionType));
    }

    public void Log<TState>
    (
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter
    )
    {
        logs.Add((logLevel, formatter.Invoke(state, exception),exception.GetType()));
    }

    [ExcludeFromCodeCoverage]
    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }
    
    [ExcludeFromCodeCoverage]
    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}
