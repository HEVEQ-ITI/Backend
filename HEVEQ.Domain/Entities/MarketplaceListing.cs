using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class MarketplaceListing
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SellerId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public ProductCondition Condition { get; set; }

    public int? YearOfManufacture { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? Specifications { get; set; }

    public decimal Price { get; set; }

    public bool IsNegotiable { get; set; }

    public MarketplaceTransactionMethod TransactionMethod { get; set; }

    public string? Governorate { get; set; }

    public string? District { get; set; }

    public MarketplaceListingStatus Status { get; set; }

    public int? AiRiskScore { get; set; }

    public string? AiRiskLevel { get; set; }

    public string? AiRiskFlags { get; set; }

    public string? VideoUrl { get; set; }

    public string? QdrantPointId { get; set; }

    public DateTime? LastEmbeddedAt { get; set; }

    public EmbeddingStatus EmbeddingStatus { get; set; }

    public string? AdminRejectionNote { get; set; }

    public Guid? RejectedByAdminId { get; set; }

    public DateTime? RejectedAt { get; set; }

    public int SubmissionCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ApplicationUser Seller { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ApplicationUser? RejectedByAdmin { get; set; }

    public ICollection<MarketplaceListingPhoto> Photos { get; set; } = new List<MarketplaceListingPhoto>();

    public ICollection<MarketplaceOrder> Orders { get; set; } = new List<MarketplaceOrder>();

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

}
