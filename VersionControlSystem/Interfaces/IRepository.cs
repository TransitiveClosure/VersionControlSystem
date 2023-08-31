namespace VersionControlSystem.Interfaces;

public interface IRepository
{
    public string RepositoryPath { get; }
    public IRepositoryObject GetIRepositoryObject(string objectPath);
    public Stream OpenWrite(string filePath);
    virtual string? GetLog(string logMessage) { return null; }
}
