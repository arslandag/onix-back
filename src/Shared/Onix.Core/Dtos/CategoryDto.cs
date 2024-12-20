namespace Onix.Core.Dtos;

public class CategoryDto
{
    public Guid Id { get; init; }
    public Guid WebSiteId { get; init; } = Guid.Empty;
    public Guid? ParentId { get; init; }

    public string Name { get; init; } = string.Empty;

    public IReadOnlyList<CategoryDto> SubCategories { get; init; } = [];
    public IReadOnlyList<ProductDto> Products { get; init; } = [];
}