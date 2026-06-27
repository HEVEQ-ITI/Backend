using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Helpers;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingCreateContext
{
    public sealed class GetBookingCreateContextQueryHandler : IRequestHandler<GetBookingCreateContextQuery, BookingCreateContextDto>
    {
        private readonly IApplicationDbContext _context;
        public GetBookingCreateContextQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingCreateContextDto> Handle(GetBookingCreateContextQuery request, CancellationToken cancellationToken)
        {
            var listing = await _context.ServiceListings
                .AsNoTracking()
                .Include(x => x.ProviderProfile)
                .Include(x => x.Availability)
                .FirstOrDefaultAsync(x => x.Id == request.ServiceListingId, cancellationToken);

            if (listing is null)
                throw new InvalidOperationException("Service listing was not found.");

            if (listing.Status != ServiceListingStatus.Active)
                throw new InvalidOperationException("Booking is allowed only for active service listings.");

            var customerProfile = await _context.CustomerProfiles
                .AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == request.CustomerId, cancellationToken);

            var defaultAddress = await _context.Addresses
                .AsNoTracking()
                .Where(x => x.UserId == request.CustomerId)
                .OrderByDescending(x => x.IsDefault)
                .ThenByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            var missingRequirements = new List<string>();

            if (customerProfile is null)
            {
                missingRequirements.Add("Customer profile is required.");
            }
            else
            {
                if (customerProfile.RequiresAdditionalVerification)
                    missingRequirements.Add("Additional verification is required.");

                if (!customerProfile.User.PhoneNumberConfirmed)
                    missingRequirements.Add("Phone number must be verified.");
            }

            if (defaultAddress is null)
                missingRequirements.Add("At least one saved address is required.");

            var availability = listing.Availability
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.OpenTime)
                .Select(x => new BookingCreateContextAvailabilityDto
                {
                    Id = x.Id,
                    DayOfWeek = x.DayOfWeek,
                    DayName = DayOfWeekDisplayHelper.GetDayName(x.DayOfWeek),
                    DayNameAr = DayOfWeekDisplayHelper.GetDayNameAr(x.DayOfWeek),
                    OpenTime = x.OpenTime,
                    CloseTime = x.CloseTime
                })
                .ToList();

            return new BookingCreateContextDto
            {
                ServiceListingId = listing.Id,
                ServiceTitle = listing.Title,
                ProviderCompany = listing.ProviderProfile.CompanyName,
                HourlyRate = listing.HourlyRate,
                DailyRate = listing.DailyRate,
                MinimumBookingHours = listing.MinimumBookingHours,
                Availability = availability,
                DefaultAddress = defaultAddress is null ? null
                    : new BookingCreateContextAddressDto
                    {
                        Id = defaultAddress.Id,
                        Label = defaultAddress.Label,
                        Governorate = defaultAddress.Governorate,
                        District = defaultAddress.District,
                        Street = defaultAddress.Street,
                        Latitude = defaultAddress.Latitude,
                        Longitude = defaultAddress.Longitude
                    },
                CustomerEligibility = new BookingCustomerEligibilityDto
                {
                    CanBook = missingRequirements.Count == 0,
                    MissingRequirements = missingRequirements
                }
            };
        }
    }
}