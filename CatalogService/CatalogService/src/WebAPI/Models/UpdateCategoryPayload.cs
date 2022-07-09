namespace WebAPI.Models;

public record UpdateCategoryPayload
{
    public string? Name { get; init; }
    public string? Image { get; init; }
    public int? ParentId { get; init; }
}
