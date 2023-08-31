using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.Cleaners;

public class RemoveCleaner : IClean
{
    public void Clean(IReadOnlyCollection<IRestorePoint> points, IBackup backup)
    {
        foreach (IRestorePoint item in points)
        {
            backup.RemoveRestorePoint(item);
        }
    }
}