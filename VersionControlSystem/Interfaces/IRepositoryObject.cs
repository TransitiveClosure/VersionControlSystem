namespace VersionControlSystem.Interfaces;

public interface IRepositoryObject
{
    public string ObjectPath { get; }
    public string Name { get; }
    public virtual List<IRepositoryObject> GetAllObjects => new List<IRepositoryObject>();
    public virtual Stream? GetStream() { return null; }
    public virtual void Add(IRepositoryObject obj) { }
    public virtual void Remove(IRepositoryObject obj) { }
    public virtual bool ItComposite() => false;
    public void Accept(IRepositoryObjectVisitor visitor);
    virtual string? GetLog(string logMessage) { return null; }
}
