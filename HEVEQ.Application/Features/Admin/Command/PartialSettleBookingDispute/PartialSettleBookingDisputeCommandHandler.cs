using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.PartialSettleBookingDispute
{
    public class PartialSettleBookingDisputeCommandHandler(
        IApplicationDbContext context)
        : IRequestHandler<PartialSettleBookingDisputeCommand, PartialSettleBookingDisputeResponse>
    {
        public async Task<PartialSettleBookingDisputeResponse> Handle(PartialSettleBookingDisputeCommand request, CancellationToken cancellationToken)
        {
            var booking = await context.Bookings
                .Include(b => b.EscrowRecords)
                .Include(b => b.ServiceListing)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return new PartialSettleBookingDisputeResponse { IsSuccess = false, StatusCode = 404, Message = "Booking not found." };
            }

            if (booking.Status != BookingStatus.Disputed)
            {
                return new PartialSettleBookingDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot settle dispute. Current booking status is {booking.Status}."
                };
            }

            var activeEscrow = booking.EscrowRecords
                .FirstOrDefault(e => e.Status == EscrowStatus.Held || e.Status == EscrowStatus.Frozen);

            if (activeEscrow == null)
            {
                return new PartialSettleBookingDisputeResponse { IsSuccess = false, StatusCode = 400, Message = "No active escrow found." };
            }

            decimal totalSettlement = request.CustomerAmount + request.ProviderAmount;
            if (totalSettlement > activeEscrow.GrossAmount)
            {
                return new PartialSettleBookingDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Total settlement amount ({totalSettlement}) exceeds the escrow gross amount ({activeEscrow.GrossAmount})."
                };
            }

            booking.Status = BookingStatus.ResolvedReleased;

            activeEscrow.Status = EscrowStatus.PartialSettled;
            activeEscrow.PartialSettleCustomerAmt = request.CustomerAmount;
            activeEscrow.PartialSettleProviderAmt = request.ProviderAmount;
            activeEscrow.ReleasedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            // 5. إرسال الإشعارات المخصصة لكلا الطرفين
            //if (notificationService != null)
            //{
            //    await notificationService.SendAsync(
            //        userId: booking.CustomerId,
            //        title: "تسوية نزاع الحجز",
            //        message: $"تم حل النزاع للحجز رقم {booking.BookingNumber} بتسوية جزئية. سيتم استرداد مبلغ {request.CustomerAmount} إليك. ملاحظة الإدارة: {request.DecisionNote}"
            //    );

            //    await notificationService.SendAsync(
            //        userId: booking.ServiceListing.ProviderProfileId,
            //        title: "تسوية نزاع الحجز",
            //        message: $"تم حل النزاع للحجز رقم {booking.BookingNumber} بتسوية جزئية. تم تحويل مبلغ {request.ProviderAmount} لمحفظتك. ملاحظة الإدارة: {request.DecisionNote}"
            //    );
            //}

            return new PartialSettleBookingDisputeResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                BookingId = booking.Id,
                Status = booking.Status.ToString(),
                StatusAr = "تم الحل بتسوية جزئية",
                EscrowStatus = activeEscrow.Status.ToString()
            };
        }
    }
}