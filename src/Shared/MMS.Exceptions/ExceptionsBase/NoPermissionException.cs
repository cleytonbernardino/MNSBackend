namespace MMS.Exceptions.ExceptionsBase;

public class NoPermissionException : MMSException
{
    public NoPermissionException() : base(ResourceMessagesException.NO_PERMISSION) { }
    public NoPermissionException(string message) : base(message) { }
}
