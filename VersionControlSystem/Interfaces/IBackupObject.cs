namespace VersionControlSystem.Interfaces;

public interface IBackupObject
{
    IRepositoryObject GetRepositoryObject();
    virtual string? GetLog(string logMessage) { return null; }
}
