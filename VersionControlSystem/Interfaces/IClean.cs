namespace VersionControlSystem.Interfaces;

public interface IClean
{
    void Clean(IReadOnlyCollection<IRestorePoint> points, IBackup backup);
}