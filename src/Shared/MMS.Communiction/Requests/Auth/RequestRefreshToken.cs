namespace MMS.Communication.Requests.Auth;

public record RequestRefreshToken
{
    public string AccessToken { get; set; } = string.Empty;
}
