using MMS.Domain.Enums;

namespace MMS.Domain.Entities;

public class User : EntityBase
{
    public bool IsAdmin { get; set; } = false;
    public DateTime UpdatedOn { get; set; }
    public DateTime LastLogin { get; set; }
    public Guid UserIdentifier { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRolesEnum Role { get; set; } = UserRolesEnum.EMPLOYEE;
    public long CompanyId { get; set; }
}
