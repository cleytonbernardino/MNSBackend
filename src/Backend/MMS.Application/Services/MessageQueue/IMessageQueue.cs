using MMS.Domain.Dtos;

namespace MMS.Application.Services.MessageQueue;

public interface IMessageQueue
{
    Task EnqueueAsync(WhatsAppMessageDto message);
    Task<WhatsAppMessageDto> DequeueAsync(CancellationToken cancellationToken);
}
