namespace MMS.Communication.Responses.Auth;

public record ResponseDoLogin
{
    public string FirstName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}
