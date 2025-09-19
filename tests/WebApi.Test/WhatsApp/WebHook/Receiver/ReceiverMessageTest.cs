using CommonTestUtilities.Requests;
using MMS.Communication.Requests.WhatsAppMessage;
using Shouldly;
using System.Net;

namespace WebApi.Test.WhatsApp.WebHook.Receiver;

public class ReceiverMessageTest(
    CustomWebApplicationFactory factory
    ) : MmsClassFixture(factory)
{

    private const string METHOD = "api/WhatsappWebHook";

    [Fact(Skip = "Desativado Temporariamente")]
    public async Task Success()
    {
        var request = RequestWhatsAppMessageBuilder.Build();

        var response = await DoPostAsync(METHOD, request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
