using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.RepositoryObjects;

public class Repository : IRepository
{
    private string _repositoryPath;

    public Repository(string repositoryPath)
    {
        _repositoryPath = repositoryPath;
    }

    public string RepositoryPath => _repositoryPath;

    public IRepositoryObject GetIRepositoryObject(string objectPath)
    {
        string absolutePath = Path.Combine(_repositoryPath, objectPath);
        if (Directory.Exists(absolutePath))
        {
            return new RepositoryDirectory(objectPath, () => GetDirectoryFiles(objectPath));
        }

        if (File.Exists(absolutePath))
        {
            return new RepositoryFile(objectPath, () => OpenFile(absolutePath));
        }

        throw new BackupExceptions("Invalid object path");
    }

    public Stream OpenWrite(string filePath)
    {
        return new FileStream(Path.Combine(_repositoryPath, filePath), FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    private Stream OpenFile(string filePath)
    {
        return File.Open(filePath, FileMode.Open);
    }

    private List<IRepositoryObject> GetDirectoryFiles(string relativePath)
    {
        string[] files = Directory.GetFiles(Path.Combine(_repositoryPath, relativePath));
        string[] directories = Directory.GetDirectories(Path.Combine(_repositoryPath, relativePath));
        var obj = files.Concat(directories).ToList();
        var repObjects = new List<IRepositoryObject>();
        obj.ForEach(name => repObjects.Add(GetIRepositoryObject(Path.Combine(relativePath, name))));
        return repObjects;
    }
}
