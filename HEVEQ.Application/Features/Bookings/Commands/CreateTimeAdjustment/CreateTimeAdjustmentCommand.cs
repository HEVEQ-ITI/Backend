using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CreateTimeAdjustment
{
    public sealed record CreateTimeAdjustmentCommand(Guid ProviderUserId, Guid BookingId, decimal AdditionalHours, string Reason) : IRequest<Guid>;
}
