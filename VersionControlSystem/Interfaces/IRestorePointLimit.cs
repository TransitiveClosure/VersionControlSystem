namespace VersionControlSystem.Interfaces;

public interface IRestorePointLimit
{
    void RemoveSuitableRestorePoints(IBackup backup);
    IReadOnlyCollection<IRestorePoint> GivePointsSuitableForCleaning(IBackup backup);
}