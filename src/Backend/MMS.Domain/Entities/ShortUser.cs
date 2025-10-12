using MMS.Domain.Enums;

namespace MMS.Domain.Entities;

public class ShortUser
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRolesEnum Role { get; set; } = UserRolesEnum.EMPLOYEE;
    public bool Active { get; set; } = false;
}
