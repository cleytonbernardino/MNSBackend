namespace MMS.Domain.Dtos;

public record WhatsAppMessageDto
{
    public required string UserIdentifier { get; init; }
    public required string Messages { get; init; }
}
