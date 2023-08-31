

using VersionControlSystem.Algorithms;
using VersionControlSystem.Entities;
using VersionControlSystem.Interfaces;
using VersionControlSystem.Services;
using Xunit;
using Zio.FileSystems;
using Assert = NUnit.Framework.Assert;

namespace VersionControlSystemUnitTest;
public class BackupTaskTest
{
    private BackupTask _backupTask = new BackupTask(new SingleAlgorithm(), new Repository(Path.Combine("D:", "Studying_at_the_university")), new Archiver());

    [Fact]
    public void CreateBackupTaskAddObjectsDoJobRemoveObjectsDoJob_ThreeStorageAndTwoRestorePoint()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.CreateDirectory(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1"));
        memoryFileSystem.CreateDirectory(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"));
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File A.txt"), FileMode.Create, FileAccess.Write).Close();
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File B.txt"), FileMode.Create, FileAccess.Write).Close();
        memoryFileSystem.OpenFile(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1", "File C.txt"), FileMode.Create, FileAccess.Write).Close();
        var rep1 = new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3", "Rep1"));
        var repTest = new MemoryRepository(memoryFileSystem, Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"));

        var backupTask = new BackupTask(new SplitAlgorithm(), repTest, new Archiver());

        IBackupObject objA = backupTask.AddBackupObject(rep1, "File A.txt");
        IBackupObject objB = backupTask.AddBackupObject(rep1, "File B.txt");

        backupTask.DoJob();

        backupTask.RemoveBackupObject(objB);

        backupTask.DoJob();

        Assert.AreEqual(2, backupTask.GetBackup.RestorePoints.Count);
        Assert.AreEqual(3, memoryFileSystem.EnumerateItems(Path.Combine(Path.DirectorySeparatorChar + "Lab3", "RepTest"), SearchOption.TopDirectoryOnly).Count());
    }
}
