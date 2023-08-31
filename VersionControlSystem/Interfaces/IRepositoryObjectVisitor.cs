using VersionControlSystem.Entities;
namespace VersionControlSystem.Interfaces;

public interface IRepositoryObjectVisitor
{
    public void VisitFile(RepositoryFile file);
    public void VisitDirectory(RepositoryDirectory directory);
}