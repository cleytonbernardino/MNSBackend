namespace MMS.Communication.Responses.User;

public record ResponseShortUser
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime? LastLogin { get; set; }
}

public record ResponseListShortUsers
{
    public List<ResponseShortUser> Users { get; set; } = [];
}
