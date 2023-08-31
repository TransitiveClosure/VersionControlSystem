using VersionControlSystem.Algorithms;
using VersionControlSystem.Entities;
using VersionControlSystem.Entities.Cleaners;
using VersionControlSystem.Entities.Logging;
using VersionControlSystem.Entities.RepositoryObjects;
using VersionControlSystem.Entities.RestorePointsLimits;
using VersionControlSystem.Interfaces;
using VersionControlSystem.Models;
using VersionControlSystem.Services;
using Xunit;
using Zio.FileSystems;

namespace VersionControlSystemUnitTest;

public class VersionControlSystemExtraTest
{
    [Fact]
    public void AddAandBObjectsDoTaskRemoveBDoTaskAddObjectCDoTaskMergeRestorePoint0and2_MergeRestorePointContain3Objects()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.CreateDirectory(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1"));
        memoryFileSystem.CreateDirectory(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"));
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File A.txt"), FileMode.Create, FileAccess.Write).Close();
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File B.txt"), FileMode.Create, FileAccess.Write).Close();
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File C.txt"), FileMode.Create, FileAccess.Write).Close();
        var rep1 = new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1"));
        var repTest = new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"));

        var backupTask = new BackupTaskExtra(new SingleAlgorithm(), repTest, new Archiver(), new FileLogger(new BaseLogerConfiguration(), new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3")), "log.log"), new AmountLimit(8, new RemoveCleaner()));

        IBackupObject objA = backupTask.AddBackupObject(rep1, "File A.txt");
        IBackupObject objB = backupTask.AddBackupObject(rep1, "File B.txt");

        backupTask.DoJob();
        backupTask.RemoveBackupObject(objB);

        backupTask.DoJob();
        IBackupObject objC = backupTask.AddBackupObject(rep1, "File C.txt");
        backupTask.DoJob();

        backupTask.Merge(0, 2);
        Assert.Equal(4, backupTask.GetBackup.RestorePoints.Count);
        Assert.Equal(3, backupTask.GetBackup.RestorePoints.Last().GetStorage.GetIRepositoryObjects().Count());
    }

    [Fact]
    public void AddAmountLimitWith2DoJobFourTimes_HaveOnlyTwoRestorePoint()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.CreateDirectory(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1"));
        memoryFileSystem.CreateDirectory(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"));
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File A.txt"), FileMode.Create, FileAccess.Write).Close();
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File B.txt"), FileMode.Create, FileAccess.Write).Close();
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File C.txt"), FileMode.Create, FileAccess.Write).Close();
        var rep1 = new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1"));
        var repTest = new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"));

        var backupTask = new BackupTaskExtra(new SingleAlgorithm(), repTest, new Archiver(), new FileLogger(new BaseLogerConfiguration(), new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3")), "log.log"), new AmountLimit(2, new RemoveCleaner()));

        IBackupObject objA = backupTask.AddBackupObject(rep1, "File A.txt");
        IBackupObject objB = backupTask.AddBackupObject(rep1, "File B.txt");

        backupTask.DoJob();
        backupTask.RemoveBackupObject(objB);

        backupTask.DoJob();
        IBackupObject objC = backupTask.AddBackupObject(rep1, "File C.txt");
        backupTask.DoJob();

        backupTask.Merge(0, 1);
        backupTask.Merge(1, 0);
        Assert.Equal(2, backupTask.GetBackup.RestorePoints.Count);
    }
}