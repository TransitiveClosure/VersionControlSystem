using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.RestorePointsLimits;

public class HybridLimitAll : IRestorePointLimit
{
    private List<IRestorePointLimit> _limits = new List<IRestorePointLimit>();
    private IClean _cleaner;

    public HybridLimitAll(IReadOnlyCollection<IRestorePointLimit> limits, IClean cleaner)
    {
        limits = _limits;
        _cleaner = cleaner;
    }

    public void RemoveSuitableRestorePoints(IBackup backup)
    {
        IReadOnlyCollection<IRestorePoint> points = GivePointsSuitableForCleaning(backup);
        _cleaner.Clean(points, backup);
    }

    public IReadOnlyCollection<IRestorePoint> GivePointsSuitableForCleaning(IBackup backup)
    {
        List<IRestorePoint> points = new List<IRestorePoint>();
        _limits.ForEach(item =>
        {
            points.Concat(item.GivePointsSuitableForCleaning(backup));
        });
        return points;
    }
}