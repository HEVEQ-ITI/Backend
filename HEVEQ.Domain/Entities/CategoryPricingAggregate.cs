using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class CategoryPricingAggregate
{
    public int CategoryId { get; set; }

    public string LocationGovernorate { get; set; } = string.Empty;

    public PriceType PriceType { get; set; }

    public decimal MedianPrice { get; set; }

    public decimal Percentile25 { get; set; }

    public decimal Percentile75 { get; set; }

    public decimal MinPrice { get; set; }

    public decimal MaxPrice { get; set; }

    public int SampleCount { get; set; }

    public DateTime ComputedAt { get; set; }
    public Category Category { get; set; } = null!;

}
