using Microsoft.Extensions.Logging;

namespace Phaneritic.Implementations.EF;

public class NoLoggerFactory 
    : ILoggerFactory
{
    private class NoLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull 
            => null;

        public bool IsEnabled(LogLevel logLevel)
            => false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
            Exception? exception, Func<TState, Exception, string> formatter)
        {
        }
    }

    public void AddProvider(ILoggerProvider provider)
    {
    }

    public ILogger CreateLogger(string categoryName) 
        => new NoLogger();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
