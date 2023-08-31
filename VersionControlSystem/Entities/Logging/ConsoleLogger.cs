using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.Logging;

public class ConsoleLogger : ILogger
{
    private ILoggerConfiguration _loggerConfiguration;
    public ConsoleLogger(ILoggerConfiguration loggerConfiguration)
    {
        _loggerConfiguration = loggerConfiguration;
    }

    public void AddLog(string logStr)
    {
        Console.WriteLine(_loggerConfiguration.StringProcessing(logStr));
    }
}