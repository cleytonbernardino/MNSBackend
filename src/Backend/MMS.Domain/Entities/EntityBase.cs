namespace MMS.Domain.Entities;

public class EntityBase<T>
{
    public T Id { get; set; }
    public DateTime CreatedOn { get; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}

public class EntityBase : EntityBase<long>
{
}
