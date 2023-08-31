using System.IO.Compression;
namespace VersionControlSystem.Interfaces;

public interface IZipRepositoryObject
{
    public string Name { get; }
    public virtual List<IRepositoryObject> GetAllObjects => new List<IRepositoryObject>();
    public IRepositoryObject GetIRepositoryObject(ZipArchiveEntry entry);
    public virtual void Add(List<IZipRepositoryObject> objects) { }
    public virtual bool ItComposite() => false;
    virtual string? GetLog(string logMessage) { return null; }
}
