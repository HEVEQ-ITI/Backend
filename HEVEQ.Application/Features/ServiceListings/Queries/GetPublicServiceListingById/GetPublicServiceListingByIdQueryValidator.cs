using FluentValidation;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListingById;

public class GetPublicServiceListingByIdQueryValidator : AbstractValidator<GetPublicServiceListingByIdQuery>
{
    public GetPublicServiceListingByIdQueryValidator()
    {
        // Validate that the incoming Guid is not empty
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Service listing ID is required.");
    }
}