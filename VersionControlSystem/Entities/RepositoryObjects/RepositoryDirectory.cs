using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.RepositoryObjects;

public class RepositoryDirectory : IRepositoryObject
{
    private Func<List<IRepositoryObject>> _getObjects;
    public RepositoryDirectory(string path, Func<List<IRepositoryObject>> funk)
    {
        ObjectPath = path;
        _getObjects = funk;
        string? name = Path.GetDirectoryName(path);
        if (name != null) Name = name;
        else throw new BackupExceptions("Invalid path to Directory");
    }

    public string ObjectPath { get; }
    public string Name { get; }
    public List<IRepositoryObject> GetAllObjects { get => _getObjects(); }
    public bool ItComposite() => true;
    public void Accept(IRepositoryObjectVisitor visitor) => visitor.VisitDirectory(this);
}
