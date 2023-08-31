using VersionControlSystem.Entities;
namespace VersionControlSystem.Interfaces;

public interface IAlgorithm
{
    public IStorage DoJob(List<IBackupObject> backupObjects, IArchiver archiver, IRepository repository);
}
