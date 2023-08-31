using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.Logging;

public class FileLogger : ILogger
{
    private ILoggerConfiguration _loggerConfiguration;
    private string _path;
    private IRepository _repository;
    public FileLogger(ILoggerConfiguration loggerConfiguration, IRepository repository, string filePath)
    {
        _loggerConfiguration = loggerConfiguration;
        _path = filePath;
        _repository = repository;
    }

    public void AddLog(string logStr)
    {
        var writer = new StreamWriter(_repository.OpenWrite(_path));
        writer.WriteLine(_loggerConfiguration.StringProcessing(logStr));
        writer.Close();
    }
}