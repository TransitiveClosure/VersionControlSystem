using System.IO.Compression;
using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class ZipRepositoryDirectory : IZipRepositoryObject
{
    private List<IZipRepositoryObject> _zipDirectoryObjects;
    public ZipRepositoryDirectory(string name, List<IZipRepositoryObject> zipObjects)
    {
        _zipDirectoryObjects = zipObjects;
        Name = name;
    }

    public List<IZipRepositoryObject> ZipDirectoryObjects => _zipDirectoryObjects;

    public string Name { get; }

    public virtual bool ItComposite() => true;
    public IRepositoryObject GetIRepositoryObject(ZipArchiveEntry entry)
    {
        return new RepositoryDirectory(entry.FullName, () => GetInsideObjects(entry));
    }

    public void Add(List<IZipRepositoryObject> objects)
    {
        _zipDirectoryObjects.Concat(objects).ToList();
    }

    private List<IRepositoryObject> GetInsideObjects(ZipArchiveEntry entry)
    {
        var repObjs = new List<IRepositoryObject>();
        using (var archive = new ZipArchive(entry.Open(), ZipArchiveMode.Read))
        {
            foreach (IZipRepositoryObject zipObj in _zipDirectoryObjects)
            {
                ZipArchiveEntry? nestedEntry = archive.GetEntry(zipObj.Name);
                if (nestedEntry == null) throw new BackupExceptions("Zip has been changed");
                if (zipObj.ItComposite())
                {
                    repObjs.Add(new ZipRepositoryDirectory(nestedEntry.Name, new List<IZipRepositoryObject>()).GetIRepositoryObject(nestedEntry));
                }
                else
                {
                    repObjs.Add(new ZipRepositoryFile(nestedEntry.Name).GetIRepositoryObject(nestedEntry));
                }
            }
        }

        return repObjs;
    }
}
