namespace MMS.Communication.Requests.ServiceDefinition;

public class RequestUpdateServiceDefinition
{
    public string Id { get; set; } = string.Empty;
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? ServiceType { get; set; } = string.Empty;
    public ushort? Status { get; set; } = null;
}
