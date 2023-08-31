using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
using Zio.FileSystems;
namespace VersionControlSystem.Entities;

public class MemoryRepository : IRepository, IDisposable
{
    private MemoryFileSystem _memoryFileSystem;
    public MemoryRepository(MemoryFileSystem system, string repositoryPath)
    {
        RepositoryPath = repositoryPath;
        _memoryFileSystem = system;
    }

    public string RepositoryPath { get; }

    public IRepositoryObject GetIRepositoryObject(string objectPath)
    {
        string absolutePath = Path.Combine(RepositoryPath, objectPath);
        if (_memoryFileSystem.DirectoryExists(absolutePath))
        {
            return new RepositoryDirectory(objectPath, () => GetDirectoryFiles(objectPath));
        }

        if (_memoryFileSystem.FileExists(absolutePath))
        {
            return new RepositoryFile(objectPath, () => OpenFile(absolutePath));
        }

        throw new BackupExceptions("Invalid object path");
    }

    public Stream OpenWrite(string filePath)
    {
        return _memoryFileSystem.OpenFile(Path.Combine(RepositoryPath, filePath), FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public void Dispose()
    {
        _memoryFileSystem.Dispose();
    }

    private Stream OpenFile(string filePath)
    {
        return _memoryFileSystem.OpenFile(filePath, FileMode.Open, FileAccess.Read);
    }

    private List<IRepositoryObject> GetDirectoryFiles(string relativePath)
    {
        IEnumerable<Zio.FileSystemItem> objs = _memoryFileSystem.EnumerateItems(Path.Combine(RepositoryPath, relativePath), SearchOption.TopDirectoryOnly);
        var repObjects = new List<IRepositoryObject>();

        foreach (Zio.FileSystemItem item in objs)
        {
            repObjects.Add(GetIRepositoryObject(Path.Combine(relativePath, item.GetName())));
        }

        return repObjects;
    }
}
