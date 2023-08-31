using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Models;

public class BaseLogerConfiguration : ILoggerConfiguration
{
    public string StringProcessing(string str)
    {
        return str;
    }
}