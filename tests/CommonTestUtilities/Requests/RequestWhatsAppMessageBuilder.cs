using Bogus;
using Google.Protobuf.Collections;
using MMS.Communication;

namespace CommonTestUtilities.Requests;

public static class RequestWhatsAppMessageBuilder
{
    public static RequestWhatsAppMessage Build()
    {
        return new Faker<RequestWhatsAppMessage>()
            .RuleFor(req => req.Object, () => "whatsapp_business_account")
            .RuleFor(req => req.Entry, (_, _) => { 
                var repeated = new RepeatedField<WhatsAppEntryRequest>
                {
                    CreateEntryObject()
                };
                return repeated; 
            })
            .Generate();
    }

    private static WhatsAppEntryRequest CreateEntryObject()
    {
        return new Faker<WhatsAppEntryRequest>()
            .RuleFor(req => req.Id, f => f.Random.Int().ToString())
            .RuleFor(req => req.Changes, (_, _) =>
            {
                var repeated = new RepeatedField<WhatsAppChangesRequest>
                {
                    CreateChangesObject()
                };
                return repeated;
            })
            .Generate();
    }

    private static WhatsAppChangesRequest CreateChangesObject()
    {
        return new Faker<WhatsAppChangesRequest>()
            .RuleFor(req => req.Field, f => f.Random.Word())
            .RuleFor(req => req.Value, () => CreateValueObject())
            .Generate();
    }

    private static WhatsAppValueRequest CreateValueObject()
    {
        return new Faker<WhatsAppValueRequest>()
            .RuleFor(req => req.MessagingProduct, f => f.Random.Word())
            .RuleFor(req => req.MetaData, () => CreateMetaDataObject())
            .RuleFor(req => req.Contacts, (f, req) =>
            {
                var repeated = new RepeatedField<WhatsAppContactsRequest>
                {
                    CreateContactsObject()
                };
                return repeated;
            })
            .RuleFor(req => req.Messages, (f, req) =>
            {
                var repeated = new RepeatedField<WhatsAppMessagesRequest>
                {
                    CreateMessagesObject()
                };
                return repeated;
            })
            .Generate();
    }

    private static WhatsAppMetaDataRequest CreateMetaDataObject()
    {
        return new Faker<WhatsAppMetaDataRequest>()
            .RuleFor(req => req.DisplayPhone, f => f.Phone.PhoneNumber("###########"))
            .RuleFor(req => req.PhoneNumberId, f => f.Random.Int(100000000).ToString())
            .Generate();
    }

    private static WhatsAppContactsRequest CreateContactsObject()
    {
        var profile = new Faker<WhatsAppProfileObject>()
            .RuleFor(profile => profile.Name, f => f.Name.FirstName())
            .Generate();

        return new Faker<WhatsAppContactsRequest>()
            .RuleFor(req => req.Profile, () => profile)
            .RuleFor(req => req.WaId, f => f.Phone.PhoneNumberFormat(11))
            .Generate();
    }

    private static WhatsAppMessagesRequest CreateMessagesObject()
    {
        var textBody = new Faker<WhatsAppTextObject>()
            .RuleFor(text => text.Body, f => f.Lorem.Paragraph())
            .Generate();

        return new Faker<WhatsAppMessagesRequest>()
            .RuleFor(req => req.From, f => f.Phone.PhoneNumber("###########"))
            .RuleFor(req => req.Id, f => f.Random.AlphaNumeric(11))
            .RuleFor(req => req.Timestamp, f => f.Random.AlphaNumeric(11))
            .RuleFor(req => req.Type, () => "text")
            .RuleFor(req => req.Text, () => textBody)
            .Generate();
    }
}
