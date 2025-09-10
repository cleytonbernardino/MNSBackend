namespace MMS.Exceptions.ExceptionsBase;

public class NotFoundException(string message) : MMSException(message)
{
}
