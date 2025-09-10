namespace MMS.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : MMSException
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> errMessages) : base("")
    {
        ErrorMessages = errMessages;
    }
}
