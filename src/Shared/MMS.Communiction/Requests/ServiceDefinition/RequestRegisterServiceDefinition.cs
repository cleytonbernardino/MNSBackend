namespace MMS.Communication.Requests.ServiceDefinition;

public class RequestRegisterServices
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public short? Status { get; set; }
}
