// ReSharper disable All
namespace MMS.Domain.ValueObjects;

public abstract class MMSConst
{
    
    /// <summary>
    /// If the refresh token expiration time is less than this time, it will regenerate the refresh token to keep the user logged in.
    /// </summary>
    public const uint SECURITY_DAYS_TO_REMAKE_TOKEN = 5;

    /// <summary>
    /// Duration that the cookie that stores the refresh token will be stored inside an httponly cookie
    /// </summary>
    public const uint REFRESH_TOKEN_COOKIE_DURATION_IN_DAYS = 10;

    /// <summary>
    /// Name that refresh cookie with httponly receives.
    /// </summary>
    public const string REFRESH_TOKEN_COOKIE_KEY = "refresh-token";
}
