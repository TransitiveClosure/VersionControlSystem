using System.IO.Compression;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class ZipRepositoryFile : IZipRepositoryObject
{
    public ZipRepositoryFile(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public IRepositoryObject GetIRepositoryObject(ZipArchiveEntry entry)
    {
        return new RepositoryFile(entry.FullName, () => entry.Open());
    }
}
