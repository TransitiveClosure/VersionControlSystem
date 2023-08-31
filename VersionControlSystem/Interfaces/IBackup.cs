namespace VersionControlSystem.Interfaces;

public interface IBackup
{
    public IReadOnlyCollection<IRestorePoint> RestorePoints { get; }
    public void AddRestorePoint(IRestorePoint restorePoint);
    public void RemoveRestorePoint(IRestorePoint restorePoint);
    public IRestorePoint GetRestorePoint(int number);
    virtual string? GetLog(string logMessage) { return null; }
}
