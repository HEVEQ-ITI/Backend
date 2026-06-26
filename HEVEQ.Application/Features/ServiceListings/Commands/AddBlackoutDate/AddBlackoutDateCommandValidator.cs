using FluentValidation;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddBlackoutDate;

public class AddBlackoutDateCommandValidator : AbstractValidator<AddBlackoutDateCommand>
{
    public AddBlackoutDateCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required.");

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Blackout date cannot be in the past.");
    }
}