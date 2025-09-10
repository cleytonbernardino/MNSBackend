using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using MMS.Application.Services.MessageQueue;
using MMS.Domain.ValueObjects;

namespace MMS.Infrastructure.Workers;

public class MessageGrouper(
    ISystemClock systemClock,
    ILogger<MessageGrouper> logger,
    IMessageQueue messageQueue
    ) : BackgroundService
{
    // To be able to send multiple requests simultaneously and advance the time in order to speed up the tests
    private readonly ISystemClock _systemClock = systemClock;

    private readonly ILogger _logger = logger;
    private readonly ConcurrentDictionary<string, UserState> _userState = new();
    private readonly IMessageQueue _messageQueue = messageQueue;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var receiver = ReceiveMessagesAsync(cancellationToken);
        var deleteExpired = RemoveExpiredMessagesAsync(cancellationToken);

        await Task.WhenAll(receiver, deleteExpired);
    }

    private async Task ReceiveMessagesAsync(CancellationToken cancellationToken) {
        while ( !cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await _messageQueue.DequeueAsync(cancellationToken);

                _userState.AddOrUpdate(
                    message.UserIdentifier,
                    addValueFactory: _ =>
                    {
                        return new UserState
                        {
                            Messages = [message.Messages],
                            ExpirationTime = _systemClock.UtcNow.AddSeconds(WhatsAppServicesConstants.WAITING_TIME_FOR_MESSAGE_PROCESSING)
                        };
                    },
                    updateValueFactory: (_, value) =>
                    {
                        value.ExpirationTime = value.ExpirationTime.AddSeconds(WhatsAppServicesConstants.WAITING_TIME_FOR_MESSAGE_PROCESSING);
                        value.Messages.Add(message.Messages);
                        return value;
                    }
                );
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Serviço de agrupar messagem foi finalizado pelo usuário.");
                break;
            }catch (Exception ex)
            {
                _logger.LogCritical($"Serviço de agrupar messagem falhou!\n\t{ex.Message}");
                break;
            }
        }
    }

    private async Task RemoveExpiredMessagesAsync(CancellationToken cancellationToken)
    {
        while ( !cancellationToken.IsCancellationRequested)
        {
            try 
            {
                await Task.Delay(
                    WhatsAppServicesConstants.WAITING_TIME_TO_CHECK_MESSAGES_TO_REMOVE, cancellationToken
                );

                foreach (var item in _userState)
                {
                    if (_systemClock.UtcNow <= item.Value.ExpirationTime) continue;
                    if (_userState.TryRemove(item.Key, out _))
                    {
                        _logger.LogInformation($"Messagem do UserID: {item.Key} foi processada\n");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Serviço de remover messagem foi finalizado pelo usuário.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Serviço de remover messagem falhou!\n\t{ex.Message}");
                break;
            }
        }
    }

    private class UserState
    {
        public List<string> Messages { get; set; } = [];
        public DateTimeOffset ExpirationTime { get; set; }
    }
}
