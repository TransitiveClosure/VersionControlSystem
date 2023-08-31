using VersionControlSystem.Entities;
namespace VersionControlSystem.Interfaces;

public interface IBackupTask
{
    public IBackup GetBackup { get; }
    public IBackupObject AddBackupObject(IRepository repository, string objectPath);
    public void RemoveBackupObject(IBackupObject backupObject);
    public void DoJob();
}
