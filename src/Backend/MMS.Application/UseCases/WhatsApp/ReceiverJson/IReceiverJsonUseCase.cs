using MMS.Communication;

namespace MMS.Application.UseCases.WhatsApp.ReceiverJson;

public interface IReceiverJsonUseCase
{
    Task Execute(RequestWhatsAppMessage request);
}
