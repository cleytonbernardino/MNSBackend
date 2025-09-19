using Microsoft.AspNetCore.Mvc;
using MMS.Application.Services.MessageQueue;
using MMS.Communication.Requests.WhatsAppMessage;
using MMS.Communication.Responses;

namespace MMS.API.Controllers;

[Route("[Controller]")]
[ApiController]
public class WhatsappWebHookController(
    IMessageQueue queue
) : ControllerBase
{
    private readonly IMessageQueue _queue = queue;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReciverMessage(
        [FromBody] RequestWhatsAppWebHook request
    )
    {
        //if (request.Entry.Count == 0)
        //    return BadRequest();

        //// Verificação Fraca, serve apenas para testes da api
        //// Refinamente é extremamente necessario
        //var dto = new WhatsAppMessageDto
        //{
        //    Messages = request.Entry[0].Changes[0].Value.Messages[0].Text.Body,
        //    UserIdentifier = request.Entry[0].Changes[0].Value.Messages[0].From
        //};

        //await _queue.EnqueueAsync(dto);
        //return Accepted();

        return NotFound();
    }
}
