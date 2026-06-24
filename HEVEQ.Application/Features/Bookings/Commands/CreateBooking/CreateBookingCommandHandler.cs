using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Application.Features.Bookings.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Commands.CreateBooking
{
    public sealed class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBookingAddressResolver _addressResolver;
        private readonly IBookingCreationService _bookingCreationService;

        public CreateBookingCommandHandler(IApplicationDbContext context, IBookingAddressResolver addressResolver, IBookingCreationService bookingCreationService)
        {
            _context = context;
            _addressResolver = addressResolver;
            _bookingCreationService = bookingCreationService;
        }

        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var customerExists = await _context.CustomerProfiles.AnyAsync(x => x.UserId == request.CustomerId, cancellationToken);
            if (!customerExists)
                throw new InvalidOperationException("Only customers can create bookings.");


            var addressSnapshot = await _addressResolver.ResolveAsync(request, cancellationToken);

            var listing = await _context.ServiceListings
                .Include(x => x.ProviderProfile)
                .Include(x => x.Availability)
                .Include(x => x.BlackoutDates)
                .FirstOrDefaultAsync(x => x.Id == request.ServiceListingId, cancellationToken);

            if (listing is null)
                throw new InvalidOperationException("Service listing was not found.");

            if (listing.ProviderProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            if (listing.ProviderProfile.UserId == request.CustomerId)
                throw new InvalidOperationException("Provider cannot book his own service listing.");

            var booking = _bookingCreationService.Create(request, listing, addressSnapshot);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);
            return booking.Id;
        }
    }
}