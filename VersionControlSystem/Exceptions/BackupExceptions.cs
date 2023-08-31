namespace VersionControlSystem.Exceptions;

public class BackupExceptions : Exception
{
    public BackupExceptions(string message)
        : base(string.Format(message)) { }
}
