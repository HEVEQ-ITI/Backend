using HEVEQ.Application.Features.Bookings.Commands.CreateBooking;

namespace HEVEQ.Application.Features.Bookings.Services.Interfaces
{
    public interface IBookingAddressResolver
    {
        Task<BookingAddressSnapshot> ResolveAsync(CreateBookingCommand request, CancellationToken cancellationToken);
    }
}