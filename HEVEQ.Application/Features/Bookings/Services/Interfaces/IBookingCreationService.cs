using HEVEQ.Application.Features.Bookings.Commands.CreateBooking;
using HEVEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Services.Interfaces
{
    public interface IBookingCreationService
    {
        Booking Create(CreateBookingCommand request, ServiceListing listing, BookingAddressSnapshot addressSnapshot);
    }
}
