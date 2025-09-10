using Moq;
using MMS.Application.Services.MessageQueue;
using MMS.Domain.Dtos;
using System.Threading.Channels;

namespace CommonTestUtilities.Services.MessageQueue;

public class MessageQueueBuilder
{
    private readonly Mock<IMessageQueue> _mock = new();

    public IMessageQueue Build() => _mock.Object;

    public MessageQueueBuilder DequeueAsync(ChannelReader<WhatsAppMessageDto> reader, CancellationToken cancellationToken)
    {
        _mock.Setup(queue => queue.DequeueAsync(It.IsAny<CancellationToken>()))
            .Returns(() => reader.ReadAsync().AsTask());
        return this;
    }
}
