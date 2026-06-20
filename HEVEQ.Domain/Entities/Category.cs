using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public CategoryType Type { get; set; }

    public int? ParentId { get; set; }
    public Category? Parent { get; set; }

    public ICollection<Category> Children { get; set; } = new List<Category>();

    public ICollection<ServiceListing> ServiceListings { get; set; } = new List<ServiceListing>();

    public ICollection<MarketplaceListing> MarketplaceListings { get; set; } = new List<MarketplaceListing>();

    public ICollection<CategoryPricingAggregate> PricingAggregates { get; set; } = new List<CategoryPricingAggregate>();

}
