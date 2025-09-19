using MMS.Communication.Requests.WhatsAppMessage;

namespace MMS.Application.UseCases.WhatsApp.ReceiverJson;

public interface IReceiverJsonUseCase
{
    Task Execute(RequestWhatsAppWebHook request);
}
