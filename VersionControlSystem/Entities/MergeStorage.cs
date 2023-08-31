using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class MergeStorage : IStorage
{
    private List<IRepositoryObject> _repositoryObjects = new List<IRepositoryObject>();
    public MergeStorage(string repositoryPath)
    {
        RepositoryPath = repositoryPath;
    }

    public string RepositoryPath { get; }
    public IReadOnlyCollection<IRepositoryObject> GetIRepositoryObjects()
    {
        return _repositoryObjects;
    }

    public void AddRepositoryObject(IRepositoryObject repositoryObject)
    {
        _repositoryObjects.Add(repositoryObject);
    }
}