namespace MMS.Communication.Requests.Auth;

public record RequestDoLogin
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
