namespace VersionControlSystem.Interfaces;

public interface IRestorePoint
{
    public string CreationTime { get; }
    public IStorage GetStorage { get; }
    virtual string? GetLog(string logMessage) { return null; }
}
