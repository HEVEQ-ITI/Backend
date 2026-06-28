using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteBlackoutDate;

public class DeleteBlackoutDateCommandValidator : AbstractValidator<DeleteBlackoutDateCommand>
{
    public DeleteBlackoutDateCommandValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.BlackoutDateId).NotEmpty();
    }
}