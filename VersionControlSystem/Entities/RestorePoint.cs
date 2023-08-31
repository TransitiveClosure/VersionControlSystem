using VersionControlSystem.Interfaces;
namespace VersionControlSystem.Entities;

public class RestorePoint : IRestorePoint
{
    private IStorage _storage;
    public RestorePoint(IStorage storage)
    {
        _storage = storage;
        DateTime dateValue = DateTime.Now;
        CreationTime = dateValue.ToString("o");
    }

    public string CreationTime { get; }
    public IStorage GetStorage => _storage;
}
