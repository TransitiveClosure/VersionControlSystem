namespace VersionControlSystem.Interfaces;

public interface IArchiver
{
    public IStorage ArchiveObjects(List<IRepositoryObject> repositoryObjects, IRepository repository);
    virtual string? GetLog(string logMessage) { return null; }
}
