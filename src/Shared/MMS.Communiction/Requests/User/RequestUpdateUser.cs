namespace MMS.Communication.Requests.User;

public record RequestUpdateUser
{
    public string UserIdToUpdate { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public short Role { get; set; }
    public string? LastName { get; set; } = string.Empty;
}

public record RequestUpdateUserPassword
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
