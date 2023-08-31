using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities.RestorePointsLimits;

public class DataLimit : IRestorePointLimit
{
    private int _numberDaysLimit;
    private IClean _cleaner;
    public DataLimit(int num, IClean cleaner)
    {
        _numberDaysLimit = num;
        _cleaner = cleaner;
    }

    public void RemoveSuitableRestorePoints(IBackup backup)
    {
        IReadOnlyCollection<IRestorePoint> points = GivePointsSuitableForCleaning(backup);
        _cleaner.Clean(points, backup);
    }

    public IReadOnlyCollection<IRestorePoint> GivePointsSuitableForCleaning(IBackup backup)
    {
        return backup.RestorePoints.Where(item => IsDataOver(item, backup.RestorePoints.Last())).ToList();
    }

    private bool IsDataOver(IRestorePoint point, IRestorePoint lastPoint)
    {
        var dateValue = DateTime.Parse(point.CreationTime);
        var dateValueLast = DateTime.Parse(lastPoint.CreationTime);
        if (dateValueLast.Day - dateValue.Day <= _numberDaysLimit) return false;
        return true;
    }
}