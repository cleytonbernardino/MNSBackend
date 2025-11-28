using MMS.Domain.Enums;

namespace MMS.Domain.Entities;

public class ServiceDefinition : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public ServicesStatusEnum Status { get; set; } = ServicesStatusEnum.PENDING;
    public long CompanyId { get; set; }
    public Guid RegisteredBy { get; set; }
}
