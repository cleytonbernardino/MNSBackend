using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Test.WhatsApp.WebHook.Receiver;

public class ReceiverMessageTest(
    CustomWebApplicationFactory factory
    ) : MmsClassFixture(factory)
{

    protected override string Method => "api/WhatsappWebHook";

    [Fact(Skip = "Desativado Temporariamente")]
    public async Task Success()
    {
        var request = RequestWhatsAppMessageBuilder.Build();

        var response = await DoPostAsync(request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
