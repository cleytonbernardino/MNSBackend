using Bogus;
using MMS.Communication.Requests.WhatsAppMessage;

namespace CommonTestUtilities.Requests;

public static class RequestWhatsAppMessageBuilder
{
    private static readonly Faker _faker = new("pt_BR");

    public static RequestWhatsAppWebHook Build()
    {
        return new Faker<RequestWhatsAppWebHook>()
            .RuleFor(o => o.Object, f => "whatsapp_business_account")
            .RuleFor(o => o.Entry, f => new List<WhatsAppEntryRequest> { GenerateEntry() });
    }

    private static WhatsAppEntryRequest GenerateEntry()
    {
        return new Faker<WhatsAppEntryRequest>()
            .RuleFor(e => e.Id, f => f.Random.Guid().ToString())
            .RuleFor(e => e.Changes, f => new List<WhatsAppChangesRequest> { GenerateChange() })
            .Generate();
    }

    private static WhatsAppChangesRequest GenerateChange()
    {
        return new Faker<WhatsAppChangesRequest>()
            .RuleFor(c => c.Field, f => "messages")
            .RuleFor(c => c.Value, f => GenerateValue())
            .Generate();
    }

    private static WhatsAppValueRequest GenerateValue()
    {
        return new Faker<WhatsAppValueRequest>()
            .RuleFor(v => v.MessagingProduct, f => "whatsapp")
            .RuleFor(v => v.MetaData, f => GenerateMetaData())
            .RuleFor(v => v.Contacts, f => new List<WhatsAppContactsRequest> { GenerateContact() })
            .RuleFor(v => v.Messages, f => new List<WhatsAppMessagesRequest> { GenerateMessage() })
            .Generate();
    }

    private static WhatsAppMetaDataRequest GenerateMetaData()
    {
        return new WhatsAppMetaDataRequest
        {
            DisplayPhone = _faker.Phone.PhoneNumber("#########"),
            PhoneNumberId = _faker.Random.Number(100000000, 999999999).ToString()
        };
    }

    private static WhatsAppContactsRequest GenerateContact()
    {
        return new WhatsAppContactsRequest
        {
            WaId = _faker.Phone.PhoneNumber("55##########"),
            Profile = new WhatsAppProfileObject { Name = _faker.Person.FullName }
        };
    }

    private static WhatsAppMessagesRequest GenerateMessage()
    {
        return new WhatsAppMessagesRequest
        {
            From = _faker.Phone.PhoneNumber("55##########"),
            Id = _faker.Random.Guid().ToString(),
            Timestamp = _faker.Date.Recent().ToUniversalTime().ToString("yyyyMMddHHmmss"),
            Type = "text",
            Text = new WhatsAppTextObject { Body = _faker.Lorem.Sentence() },
            Context = GenerateContext()
        };
    }

    private static WhatsAppContextObject GenerateContext()
    {
        return new WhatsAppContextObject
        {
            From = _faker.Phone.PhoneNumber("55##########"), Id = _faker.Random.Guid().ToString()
        };
    }
}
