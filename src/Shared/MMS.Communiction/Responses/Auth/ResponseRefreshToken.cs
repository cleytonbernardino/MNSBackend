namespace MMS.Communication.Responses.Auth;

public record ResponseRefreshToken
{
    public string AccessToken { get; set; } = string.Empty;
}
