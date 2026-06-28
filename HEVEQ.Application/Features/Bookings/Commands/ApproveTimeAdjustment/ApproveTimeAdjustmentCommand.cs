using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.ApproveTimeAdjustment
{
    public sealed record ApproveTimeAdjustmentCommand(Guid CustomerId, Guid TimeAdjustmentRequestId) : IRequest<TimeAdjustmentDecisionResponseDto>;
}
