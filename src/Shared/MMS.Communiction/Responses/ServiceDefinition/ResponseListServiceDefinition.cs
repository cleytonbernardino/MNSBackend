namespace MMS.Communication.Responses.ServiceDefinition;

public record ResponseShortServicesDefinition
{
    public ResponseShortServiceDefinition[] ServiceDefinitions { get; init; } = [];
};

public record ResponseShortServiceDefinition
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
