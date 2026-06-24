using FluentValidation;

namespace HEVEQ.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.ServiceListingId)
                .NotEmpty()
                .WithMessage("ServiceListingId is required.");

            RuleFor(x => x.JobTitle)
                .NotEmpty()
                .WithMessage("Job title is required.")
                .MaximumLength(150)
                .WithMessage("Job title cannot exceed 150 characters.");

            RuleFor(x => x.JobDescription)
                .MaximumLength(1000)
                .WithMessage("Job description cannot exceed 1000 characters.");

            RuleFor(x => x.RequestedStartDate)
                .NotEmpty()
                .WithMessage("Requested start date is required.");

            RuleFor(x => x.RequestedStartTime)
                .NotEmpty()
                .WithMessage("Requested start time is required.");

            RuleFor(x => x.EstimatedDurationHours)
                .GreaterThan(0)
                .WithMessage("Estimated duration hours must be greater than 0.");

            RuleFor(x => x.SiteContactName)
                .MaximumLength(150)
                .WithMessage("Site contact name cannot exceed 150 characters.");

            RuleFor(x => x.SiteContactPhone)
                .MaximumLength(11)
                .WithMessage("Site contact phone cannot exceed 11 numbers.");

            RuleFor(x => x.AccessRequirements)
                .MaximumLength(500)
                .WithMessage("Access requirements cannot exceed 500 characters.");

            RuleFor(x => x.SafetyNotes)
                .MaximumLength(500)
                .WithMessage("Safety notes cannot exceed 500 characters.");

            When(x => x.AddressId is null, () =>
            {
                RuleFor(x => x.Governorate)
                    .NotEmpty()
                    .WithMessage("Governorate is required when no saved address is selected.")
                    .MaximumLength(100)
                    .WithMessage("Governorate cannot exceed 100 characters.");

                RuleFor(x => x.District)
                    .NotEmpty()
                    .WithMessage("District is required when no saved address is selected.")
                    .MaximumLength(100)
                    .WithMessage("District cannot exceed 100 characters.");

                RuleFor(x => x.Street)
                    .MaximumLength(250)
                    .WithMessage("Street cannot exceed 250 characters.");

                RuleFor(x => x.Latitude)
                    .NotNull()
                    .WithMessage("Latitude is required when no saved address is selected.")
                    .InclusiveBetween(-90, 90)
                    .WithMessage("Latitude must be between -90 and 90.");

                RuleFor(x => x.Longitude)
                    .NotNull()
                    .WithMessage("Longitude is required when no saved address is selected.")
                    .InclusiveBetween(-180, 180)
                    .WithMessage("Longitude must be between -180 and 180.");
            });

            When(x => x.AddressId is not null, () =>
            {
                RuleFor(x => x.Latitude)
                    .Null()
                    .WithMessage("Latitude should not be sent when Address is selected.");

                RuleFor(x => x.Longitude)
                    .Null()
                    .WithMessage("Longitude should not be sent when Address is selected.");
            });
        }
    }
}