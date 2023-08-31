using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class BackupObject : IBackupObject
{
    private IRepository _repository;
    private string _objectPath;

    public BackupObject(IRepository repository, string objectPath)
    {
        _repository = repository;
        _objectPath = objectPath;
    }

    public IRepositoryObject GetRepositoryObject()
    {
        return _repository.GetIRepositoryObject(_objectPath);
    }
}
