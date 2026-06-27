using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Commands.SubmitServiceListingForReview
{
    public record SubmitServiceListingForReviewCommand(Guid Id) : IRequest<SubmitForReviewResultDto>;
}
