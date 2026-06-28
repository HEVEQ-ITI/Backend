using FluentValidation;
using HEVEQ.Application.Features.ServiceListings.Queries;
using HEVEQ.Application.Features.ServiceListings.Queries.GetProviderServiceListings;
using HEVEQ.Domain.Enums;
using System;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderServiceListings;

/// <summary>
/// Validator for the provider service listings query.
/// Ensures that the status filter provided is valid according to the system enum.
/// </summary>
public class GetProviderServiceListingsQueryValidator : AbstractValidator<GetProviderServiceListingsQuery>
{
    public GetProviderServiceListingsQueryValidator()
    {
        RuleFor(x => x.Status)
                    .IsInEnum()
                    .WithMessage("The provided status filter is invalid. Please use a valid service listing status.")
                    .When(x => x.Status.HasValue);
    }
}
