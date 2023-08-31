using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class Unarchiver
{
    public void Unarchive(IStorage storage, IRepository repository)
    {
        var visitor = new UnarchiveRepositoryObjectVisitor(repository);
        foreach (IRepositoryObject repositoryObject in storage.GetIRepositoryObjects())
        {
            repositoryObject.Accept(visitor);
        }
    }
}