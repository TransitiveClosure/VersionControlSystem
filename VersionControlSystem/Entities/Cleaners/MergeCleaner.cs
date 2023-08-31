using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.Cleaners;

public class MergeCleaner : IClean
{
    public void Clean(IReadOnlyCollection<IRestorePoint> points, IBackup backup)
    {
        var storage = new MergeStorage(points.First().GetStorage.RepositoryPath);
        foreach (IRestorePoint item in points.OrderByDescending(p => p.CreationTime))
        {
            foreach (IRepositoryObject obj in item.GetStorage.GetIRepositoryObjects())
            {
                if (storage.GetIRepositoryObjects().All(p => p.Name != obj.Name)) storage.AddRepositoryObject(obj);
            }

            backup.RemoveRestorePoint(item);
        }

        backup.AddRestorePoint(new RestorePoint(storage));
    }
}