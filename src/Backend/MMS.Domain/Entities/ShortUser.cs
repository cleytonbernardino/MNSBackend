using MMS.Domain.Enums;

namespace MMS.Domain.Entities;

public class ShortUser
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public UserRolesEnum Role { get; set; } = UserRolesEnum.EMPLOYEE;
    public string? LastName { get; set; }
    public DateTime? LastLogin { get; set; }
}
