using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
using VersionControlSystem.Entities.RepositoryObjects;
namespace VersionControlSystem.Entities;

public class UnarchiveRepositoryObjectVisitor : IRepositoryObjectVisitor
{
    private IRepository _repository;
    public UnarchiveRepositoryObjectVisitor(IRepository repository)
    {
        _repository = repository;
    }

    public void VisitFile(RepositoryFile file)
    {
        Stream inFile = _repository.OpenWrite(file.ObjectPath);
        Stream? zipFile = file.GetStream;
        if (zipFile is null) throw new BackupExceptions("Incorrect unarhive data");
        var reader = new StreamReader(zipFile);
        string line = reader.ReadToEnd();
        var writer = new StreamWriter(inFile);
        writer.WriteLine(line);
        reader.Close();
        writer.Close();
    }

    public void VisitDirectory(RepositoryDirectory directory)
    {
        directory.GetAllObjects.ForEach(item =>
        {
            item.Accept(this);
        });
    }
}