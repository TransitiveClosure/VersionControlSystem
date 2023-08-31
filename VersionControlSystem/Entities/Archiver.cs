using System.IO.Compression;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;
public class Archiver : IArchiver
{
    public IStorage ArchiveObjects(List<IRepositoryObject> repositoryObjects, IRepository repository)
    {
        Thread.Sleep(1);
        string archivePath = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "__" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond;
        using (var archive = new ZipArchive(repository.OpenWrite(archivePath), ZipArchiveMode.Create))
        {
            var visitor = new RepositoryObjectVisitor(archive);
            repositoryObjects.ForEach(obj => AddObject(obj, visitor));
            var zipStorage = new ZipStorage(repository, archivePath, visitor.ZipObjects);
            return zipStorage;
        }
    }

    private void AddObject(IRepositoryObject repositoryObject, RepositoryObjectVisitor visitor)
    {
        repositoryObject.Accept(visitor);
        if (repositoryObject.ItComposite())
        {
            repositoryObject.GetAllObjects.ForEach(repObj => AddObject(repObj, visitor));
            visitor.MoveUpDirectory();
        }
    }
}
