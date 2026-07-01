using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Reviews.DTOs;

namespace HEVEQ.Application.Features.Reviews.Queries.GetUserReviews;

public class GetUserReviewsQueryHandler
    : IRequestHandler<GetUserReviewsQuery, ReviewListDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReviewListDto> Handle(
        GetUserReviewsQuery request,
        CancellationToken cancellationToken)
    {
        // Business Rule: public endpoint — no auth required
        // Business Rule: only published reviews returned
        var reviews = await _context.Reviews
            .AsNoTracking()
            .Where(r => r.ReviewedUserId == request.UserId && r.IsPublished)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                ReviewerName = r.Reviewer.FirstName + " " + r.Reviewer.LastName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var totalCount = reviews.Count;
        var averageRating = totalCount > 0
            ? Math.Round((decimal)reviews.Average(r => r.Rating), 1)
            : 0m;

        return new ReviewListDto
        {
            Items = reviews,
            AverageRating = averageRating,
            TotalCount = totalCount
        };
    }
}