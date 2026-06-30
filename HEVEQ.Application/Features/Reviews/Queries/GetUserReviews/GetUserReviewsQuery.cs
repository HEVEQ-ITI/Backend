using HEVEQ.Application.Features.Reviews.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Reviews.Queries.GetUserReviews;

public record GetUserReviewsQuery(Guid UserId) : IRequest<ReviewListDto>;