namespace MMS.Domain.Entities;

public class EntityBaseShortId
{
    public short Id { get; set; }
    public DateTime CreatedOn { get; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
