using System.Reflection;
using System.Text.Json;
using VersionControlSystem.Algorithms;
using VersionControlSystem.Entities;
using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Services;

public class BackupTaskExtra
{
    private IAlgorithm _algorithm;
    private IRepository _repository;
    private IArchiver _archiver;
    private List<IBackupObject> _backupObjects = new List<IBackupObject>();
    private BackupExtra _backup;
    private ILogger _logger;
    private Unarchiver _unarchiver = new Unarchiver();
    public BackupTaskExtra(IAlgorithm algorithm, IRepository repository, IArchiver archiver, ILogger logger, IRestorePointLimit restorePointLimit)
    {
        _algorithm = algorithm;
        _repository = repository;
        _archiver = archiver;
        _backup = new BackupExtra(restorePointLimit, logger);
        _logger = logger;
        UploadData();
    }

    public IBackup GetBackup => _backup;

    public IBackupObject AddBackupObject(IRepository repository, string objectPath)
    {
        _backupObjects.Add(new BackupObject(repository, objectPath));
        _logger.AddLog($"Add BackupObject with path: {objectPath}");
        return _backupObjects.Last();
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        _logger.AddLog($"Remove BackupObject with path: {backupObject.GetRepositoryObject().ObjectPath}");
        _backupObjects.Remove(backupObject);
    }

    public void DoJob()
    {
        IStorage storage = _algorithm.DoJob(_backupObjects, _archiver, _repository);
        _logger.AddLog($"Create Sorage with path: {storage.RepositoryPath}");
        _backup.AddRestorePoint(new RestorePoint(storage));
    }

    public List<string> GetRestorePoints()
    {
        var points = new List<string>();
        foreach (IRestorePoint item in _backup.RestorePoints)
        {
            points.Add(item.CreationTime);
        }

        return points;
    }

    public void DataRecovery(int restorePoint)
    {
        _unarchiver.Unarchive(GetRestorePoint(restorePoint).GetStorage, _repository);
    }

    public void DataRecovery(int restorePoint, IRepository repository)
    {
        _unarchiver.Unarchive(GetRestorePoint(restorePoint).GetStorage, repository);
    }

    public void Merge(int restorePointNew, int restorePointOld)
    {
        var storage = new MergeStorage(GetRestorePoint(restorePointNew).GetStorage.RepositoryPath);
        IReadOnlyCollection<IRepositoryObject> repositoryObjectsNew = GetRestorePoint(restorePointNew).GetStorage.GetIRepositoryObjects();
        foreach (IRepositoryObject point in repositoryObjectsNew)
        {
            storage.AddRepositoryObject(point);
        }

        IReadOnlyCollection<IRepositoryObject> repositoryObjectsOld = GetRestorePoint(restorePointOld).GetStorage.GetIRepositoryObjects();
        foreach (IRepositoryObject point in repositoryObjectsOld)
        {
            if (repositoryObjectsNew.All(item => item.Name != point.Name)) storage.AddRepositoryObject(point);
        }

        _backup.AddRestorePoint(new RestorePoint(storage));
    }

    public void SafeData()
    {
        using Stream createStream = _repository.OpenWrite("Config.json");
        JsonSerializer.Serialize(createStream, _backup);
    }

    private IRestorePoint GetRestorePoint(int number)
    {
       return _backup.GetRestorePoint(number);
    }

    private void UploadData()
    {
        try
        {
            IRepositoryObject repositoryObject = _repository.GetIRepositoryObject("Config.json");
            BackupExtra? backupTaskExtra = JsonSerializer.Deserialize<BackupExtra>(repositoryObject.GetStream() !);
            if (backupTaskExtra is not null) _backup = backupTaskExtra;
        }
        catch (BackupExceptions)
        {
        }
    }
}