using VersionControlSystem.Algorithms;
using VersionControlSystem.Entities;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Services;

public class BackupTask : IBackupTask
{
    private IAlgorithm _algorithm;
    private IRepository _repository;
    private IArchiver _archiver;
    private List<IBackupObject> _backupObjects = new List<IBackupObject>();
    private Backup _backup = new Backup();
    public BackupTask(IAlgorithm algorithm, IRepository repository, IArchiver archiver)
    {
        _algorithm = algorithm;
        _repository = repository;
        _archiver = archiver;
    }

    public IBackup GetBackup => _backup;

    public IBackupObject AddBackupObject(IRepository repository, string objectPath)
    {
        _backupObjects.Add(new BackupObject(repository, objectPath));
        return _backupObjects.Last();
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        _backupObjects.Remove(backupObject);
    }

    public void DoJob()
    {
        IStorage storage = _algorithm.DoJob(_backupObjects, _archiver, _repository);
        _backup.AddRestorePoint(new RestorePoint(storage));
    }
}
