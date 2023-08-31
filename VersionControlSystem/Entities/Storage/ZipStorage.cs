using System.IO.Compression;
using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.Storage;

public class ZipStorage : IStorage
{
    private List<IZipRepositoryObject> _repositoryObjects;
    private string _archivePath;
    private IRepository _repository;

    public ZipStorage(IRepository repository, string archivePath, List<IZipRepositoryObject> repositoryObjects)
    {
        _repositoryObjects = repositoryObjects;
        _archivePath = archivePath;
        _repository = repository;
    }

    public string RepositoryPath => _repository.RepositoryPath;

    public IReadOnlyCollection<IRepositoryObject> GetIRepositoryObjects()
    {
        using (var archive = new ZipArchive(_repository.OpenWrite(_archivePath), ZipArchiveMode.Read))
        {
            var repositoryObjects = new List<IRepositoryObject>();
            foreach (IZipRepositoryObject obj in _repositoryObjects)
            {
                ZipArchiveEntry? entry = archive.GetEntry(obj.Name);
                if (entry == null) throw new BackupExceptions("Zip has been changed");
                repositoryObjects.Add(obj.GetIRepositoryObject(entry));
            }

            return repositoryObjects;
        }
    }
}
