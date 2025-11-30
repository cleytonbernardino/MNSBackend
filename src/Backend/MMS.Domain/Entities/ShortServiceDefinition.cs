namespace MMS.Domain.Entities;

public class ShortServiceDefinition
{
    public string Title { get; set; } = string.Empty;

    public string? Description
    {
        get
        {
            return Description ?? string.Empty;
        }
        set => Description = value;
    }
}
