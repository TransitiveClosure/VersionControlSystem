using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class RepositoryFile : IRepositoryObject
{
    private Func<Stream> _openFile;
    public RepositoryFile(string path, Func<Stream> openFile)
    {
        ObjectPath = path;
        _openFile = openFile;
        Name = Path.GetFileName(path);
    }

    public string Name { get; }
    public string ObjectPath { get; }
    public Stream GetStream => _openFile();
    public void Accept(IRepositoryObjectVisitor visitor) => visitor.VisitFile(this);
}
