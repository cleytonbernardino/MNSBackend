namespace MMS.Communication.Responses.ServiceDefinition;

public record ResponseGetServiceDefinition
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ServiceType { get; init; } = string.Empty;
    public short Status { get; init; }
}
