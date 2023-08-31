using System.IO.Compression;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.RepositoryObjects;

public class RepositoryObjectVisitor : IRepositoryObjectVisitor
{
    private Stack<ZipArchive> _archives = new Stack<ZipArchive>();
    private Stack<List<IZipRepositoryObject>> _zipObjects = new Stack<List<IZipRepositoryObject>>();
    public RepositoryObjectVisitor(ZipArchive archive)
    {
        _archives.Push(archive);
        _zipObjects.Push(new List<IZipRepositoryObject>());
    }

    public List<IZipRepositoryObject> ZipObjects => _zipObjects.Peek();
    public void VisitFile(RepositoryFile file)
    {
        _zipObjects.Peek().Add(new ZipRepositoryFile(file.Name));
        ZipArchiveEntry entry = _archives.Peek().CreateEntry(file.Name);
        var reader = new StreamReader(file.GetStream);
        string line = reader.ReadToEnd();
        var writer = new StreamWriter(entry.Open());
        writer.WriteLine(line);
        reader.Close();
        writer.Close();
    }

    public void VisitDirectory(RepositoryDirectory directory)
    {
        ZipArchiveEntry entry = _archives.Peek().CreateEntry(directory.Name);
        _archives.Push(new ZipArchive(entry.Open(), ZipArchiveMode.Create));
        _zipObjects.Peek().Add(new ZipRepositoryDirectory(directory.Name, new List<IZipRepositoryObject>()));
        _zipObjects.Push(new List<IZipRepositoryObject>());
    }

    public void MoveUpDirectory()
    {
        if (_archives.Count() > 1)
        {
            _archives.Peek().Dispose();
            _archives.Pop();
            List<IZipRepositoryObject> zipDir = _zipObjects.Peek();
            _zipObjects.Pop();
            _zipObjects.Peek().Last().Add(zipDir);
        }
    }
}
