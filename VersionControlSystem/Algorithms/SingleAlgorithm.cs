using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Algorithms;

public class SingleAlgorithm : IAlgorithm
{
    public IStorage DoJob(List<IBackupObject> backupObjects, IArchiver archiver, IRepository repository)
    {
        var repositoryObjects = new List<IRepositoryObject>();
        backupObjects.ForEach(obj => repositoryObjects.Add(obj.GetRepositoryObject()));
        IStorage storage = archiver.ArchiveObjects(repositoryObjects, repository);
        return storage;
    }
}
