using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.Commands.CreateBooking;
using HEVEQ.Application.Features.Bookings.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Services.Implementation
{
    public class BookingAddressResolver : IBookingAddressResolver
    {
        private readonly IApplicationDbContext _context;

        public BookingAddressResolver(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingAddressSnapshot> ResolveAsync(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.AddressId is not null)
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(x =>
                    x.Id == request.AddressId &&
                    x.UserId == request.CustomerId, cancellationToken);

                if (address is null)
                    throw new InvalidOperationException("Selected address was not found for this customer.");

                if (address.Latitude is null || address.Longitude is null)
                    throw new InvalidOperationException("Selected address must have latitude and longitude.");

                return new BookingAddressSnapshot
                {
                    Governorate = address.Governorate,
                    District = address.District,
                    Street = address.Street,
                    Latitude = address.Latitude.Value,
                    Longitude = address.Longitude.Value
                };
            }

            if (string.IsNullOrWhiteSpace(request.Governorate) ||
                string.IsNullOrWhiteSpace(request.District))
            {
                throw new InvalidOperationException("Governorate and district are required when no saved address is selected.");
            }

            if (request.Latitude is null || request.Longitude is null)
                throw new InvalidOperationException("Latitude and longitude are required when no saved address is selected.");

            return new BookingAddressSnapshot
            {
                Governorate = request.Governorate.Trim(),
                District = request.District.Trim(),
                Street = request.Street,
                Latitude = request.Latitude.Value,
                Longitude = request.Longitude.Value
            };
        }
    }
}
