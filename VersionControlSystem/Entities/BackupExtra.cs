using VersionControlSystem.Exceptions;
using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class BackupExtra : IBackup
{
    private IRestorePointLimit _restorePointsLimit;
    private ILogger _logger;
    private List<IRestorePoint> _restorePoints = new List<IRestorePoint>();
    public BackupExtra(IRestorePointLimit restorePointsLimit, ILogger logger)
    {
        _restorePointsLimit = restorePointsLimit;
        _logger = logger;
    }

    public IReadOnlyCollection<IRestorePoint> RestorePoints => _restorePoints;
    public void AddRestorePoint(IRestorePoint restorePoint)
    {
        _restorePoints.Add(restorePoint);
        _logger.AddLog($"Add Restory point: {restorePoint.CreationTime}; for storage with path: {restorePoint.GetStorage.RepositoryPath}");
        _restorePointsLimit.RemoveSuitableRestorePoints(this);
    }

    public void RemoveRestorePoint(IRestorePoint restorePoint)
    {
        _logger.AddLog($"Remove Restory point: {restorePoint.CreationTime}; for storage with path: {restorePoint.GetStorage.RepositoryPath}");
        _restorePoints.Remove(restorePoint);
    }

    public IRestorePoint GetRestorePoint(int number)
    {
        if (_restorePoints.Count() < number || number < 0)
            throw new BackupExceptions("Incorrect number restore point");
        else return _restorePoints[number];
    }
}