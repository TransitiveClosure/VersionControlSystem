namespace VersionControlSystem.Interfaces;

public interface IStorage
{
    public string RepositoryPath { get; }
    IReadOnlyCollection<IRepositoryObject> GetIRepositoryObjects();
    virtual string? GetLog(string logMessage) { return null; }
}
