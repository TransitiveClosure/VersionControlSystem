using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.Storage;

public class SplitStorage : IStorage
{
    private List<IStorage> _storages;
    private IRepository _repository;

    public SplitStorage(IRepository repository, List<IStorage> storages)
    {
        _storages = storages;
        _repository = repository;
    }

    public string RepositoryPath => _repository.RepositoryPath;

    public IReadOnlyCollection<IRepositoryObject> GetIRepositoryObjects()
    {
        var repositoryObjects = new List<IRepositoryObject>();
        _storages.ForEach(storage => repositoryObjects.Concat(storage.GetIRepositoryObjects()).ToString());
        return repositoryObjects;
    }
}
