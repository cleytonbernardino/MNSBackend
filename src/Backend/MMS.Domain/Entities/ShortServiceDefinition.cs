namespace MMS.Domain.Entities;

public class ShortServiceDefinition
{
    private string _description = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Description
    {
        get => _description;
        set
        {
            _description = string.IsNullOrEmpty(value) ? string.Empty : value;
        }
    }
}
