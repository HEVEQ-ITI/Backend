using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.RejectTimeAdjustment
{
    public sealed record RejectTimeAdjustmentCommand(Guid CustomerId, Guid TimeAdjustmentRequestId) : IRequest<BookingDto>;
}
