namespace MMS.Communication.Responses;

public record ResponseError
{
    public List<string> Errors { get; set; } = [];
}
