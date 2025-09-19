namespace MMS.Communication.Requests.WhatsAppMessage;

public record RequestWhatsAppWebHook
{
    public string Object { get; set; }
    public List<WhatsAppEntryRequest> Entry { get; set; }
}

public record WhatsAppEntryRequest
{
    public string Id { get; set; }
    public List<WhatsAppChangesRequest> Changes { get; set; }
}

public record WhatsAppChangesRequest
{
    public string Field { get; set; }
    public WhatsAppValueRequest Value { get; set; }
}

public record WhatsAppMetaDataRequest
{
    public string DisplayPhone { get; set; }
    public string PhoneNumberId { get; set; }
}

public record WhatsAppProfileObject
{
    public string Name { get; set; }
}

public record WhatsAppContactsRequest
{
    public WhatsAppProfileObject Profile { get; set; }
    public string WaId { get; set; }
}

public record WhatsAppTextObject
{
    public string Body { get; set; }
}

public record WhatsAppContextObject
{
    public string From { get; set; }
    public string Id { get; set; }
}

public record WhatsAppMessagesRequest
{
    public string From { get; set; }
    public string Id { get; set; }
    public WhatsAppContextObject Context { get; set; }
    public string Timestamp { get; set; }
    public string Type { get; set; }
    public WhatsAppTextObject Text { get; set; }
}

public record WhatsAppValueRequest
{
    public string MessagingProduct { get; set; }
    public WhatsAppMetaDataRequest MetaData { get; set; }
    public List<WhatsAppContactsRequest> Contacts { get; set; }
    public List<WhatsAppMessagesRequest> Messages { get; set; }
}
