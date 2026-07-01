namespace HEVEQ.Application.Features.Reviews.DTOs;

// Used by GET /api/reviews/for-user/{userId}
public class ReviewDto
{
    public Guid Id { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ReviewListDto
{
    public List<ReviewDto> Items { get; set; } = new();
    public decimal AverageRating { get; set; }
    public int TotalCount { get; set; }
}

// Used by POST /api/reviews
public class SubmitReviewResult
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public bool IsPublished { get; set; }
    public string Message { get; set; } = string.Empty;
}