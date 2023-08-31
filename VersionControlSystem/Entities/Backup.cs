using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class Backup : IBackup
{
    private List<IRestorePoint> _restorePoints = new List<IRestorePoint>();
    public IReadOnlyCollection<IRestorePoint> RestorePoints => _restorePoints;
    public void AddRestorePoint(IRestorePoint restorePoint)
    {
        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(IRestorePoint restorePoint)
    {
        _restorePoints.Remove(restorePoint);
    }

    public IRestorePoint GetRestorePoint(int number)
    {
        if (_restorePoints.Count() < number || number < 0)
            throw new BackupExceptions("Incorrect number restore point");
        else return _restorePoints[number];
    }
}
