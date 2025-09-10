namespace MMS.Domain.ValueObjects;

public abstract class MMSConst
{
    
    /// <summary>
    /// If the refresh token expiration time is less than this time, it will regenerate the refresh token to keep the user logged in.
    /// </summary>
    public const uint SECURITY_DAYS_TO_REMAKE_TOKEN = 5;
}
