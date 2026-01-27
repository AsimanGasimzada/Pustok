using Pustok.Core.Entites.Common;

namespace Pustok.Core.Entites;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = [];
}
