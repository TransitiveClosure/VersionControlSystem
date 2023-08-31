using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.RestorePointsLimits;

public class AmountLimit : IRestorePointLimit
{
    private int amountLimit;
    private IClean _cleaner;
    public AmountLimit(int num, IClean cleaner)
    {
        amountLimit = num;
        _cleaner = cleaner;
    }

    public void RemoveSuitableRestorePoints(IBackup backup)
    {
        IReadOnlyCollection<IRestorePoint> points = GivePointsSuitableForCleaning(backup);
        _cleaner.Clean(points, backup);
    }

    public IReadOnlyCollection<IRestorePoint> GivePointsSuitableForCleaning(IBackup backup)
    {
        if (backup.RestorePoints.Count <= amountLimit)
            return new List<IRestorePoint>();
        return backup.RestorePoints.OrderBy(item => item.CreationTime).SkipLast(amountLimit).ToList();
    }
}