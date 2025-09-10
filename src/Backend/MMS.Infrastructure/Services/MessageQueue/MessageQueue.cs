using System.Threading.Channels;
using MMS.Application.Services.MessageQueue;
using MMS.Domain.Dtos;

namespace MMS.Infrastructure.Services.MessageQueue;

public class MessageQueue : IMessageQueue
{
    private readonly Channel<WhatsAppMessageDto> _channel = Channel.CreateUnbounded<WhatsAppMessageDto>();

    public async Task<WhatsAppMessageDto> DequeueAsync(CancellationToken cancellationToken) => await _channel.Reader.ReadAsync(cancellationToken);

    public async Task EnqueueAsync(WhatsAppMessageDto message) => await _channel.Writer.WriteAsync(message);
}
