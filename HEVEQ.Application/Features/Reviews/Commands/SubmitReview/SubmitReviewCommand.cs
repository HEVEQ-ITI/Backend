using HEVEQ.Application.Features.Reviews.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Reviews.Commands.SubmitReview;

public record SubmitReviewCommand(
    Guid? BookingId,
    Guid? MarketplaceOrderId,
    int Rating,
    string? Comment
) : IRequest<SubmitReviewResult>;