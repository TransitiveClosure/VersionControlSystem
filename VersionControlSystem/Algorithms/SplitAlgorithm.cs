using VersionControlSystem.Entities;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Algorithms;

public class SplitAlgorithm : IAlgorithm
{
    public IStorage DoJob(List<IBackupObject> backupObjects, IArchiver archiver, IRepository repository)
    {
        var storages = new List<IStorage>();
        backupObjects.ForEach(obj => storages.Add(archiver.ArchiveObjects(new List<IRepositoryObject>() { obj.GetRepositoryObject() }, repository)));
        return new SplitStorage(repository, storages);
     }
}
