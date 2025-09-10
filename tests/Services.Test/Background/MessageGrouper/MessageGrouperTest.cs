using CommonTestUtilities.DotNetInterfaces;
using CommonTestUtilities.Dto;
using CommonTestUtilities.Services.MessageQueue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using System.Threading.Channels;
using MMS.Domain.Dtos;
using Workers = MMS.Infrastructure.Workers;

namespace Services.Test.Background.MessageGrouper;

public class MessageGrouperTest
{
    [Fact(Skip = "Serviço desativado temporariamente.")]
    public async Task Success_Message_Is_Being_Delivered()
    {
        CancellationTokenSource cancellationToken = new();
        var channel = Channel.CreateUnbounded<WhatsAppMessageDto>();
        var fakeClock = new FakeSystemClock();
        ConfigureScopteFactory();

        var service = CreateService(fakeClock, channel, cancellationToken.Token);
        await service.StartAsync(cancellationToken.Token);
        await channel.Writer.WriteAsync(WhatsAppMessageDtoBuilder.Build());

        cancellationToken.Cancel();

        channel.Reader.Count.ShouldBe(0);
    }

    private static void ConfigureScopteFactory()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        var scope = new Mock<IServiceScope>();
        var scopeFactory = new Mock<IServiceScopeFactory>();
        scopeFactory.Setup(s => s.CreateScope()).Returns(scope.Object);
        scope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory.Object);
    }

    private static Workers.MessageGrouper CreateService(
        FakeSystemClock fakeClock, Channel<WhatsAppMessageDto> channel, CancellationToken cancellationToken
        )
    {
        var queue = new MessageQueueBuilder();

        queue.DequeueAsync(channel.Reader, cancellationToken);

        return new Workers.MessageGrouper(
            fakeClock, new NullLogger<Workers.MessageGrouper>(), queue.Build()
            );
    }
}
