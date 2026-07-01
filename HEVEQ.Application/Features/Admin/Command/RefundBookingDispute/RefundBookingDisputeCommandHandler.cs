using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RefundBookingDispute
{
    public class RefundBookingDisputeCommandHandler(
        IApplicationDbContext context)
        : IRequestHandler<RefundBookingDisputeCommand, RefundBookingDisputeResponse>
    {
        public async Task<RefundBookingDisputeResponse> Handle(RefundBookingDisputeCommand request, CancellationToken cancellationToken)
        {
            var booking = await context.Bookings
                .Include(b => b.EscrowRecords)
                .Include(b => b.ServiceListing)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return new RefundBookingDisputeResponse { IsSuccess = false, StatusCode = 404, Message = "Booking not found." };
            }

            if (booking.Status != BookingStatus.Disputed)
            {
                return new RefundBookingDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot refund dispute. Current booking status is {booking.Status}."
                };
            }

            var activeEscrow = booking.EscrowRecords
                .FirstOrDefault(e => e.Status == EscrowStatus.Held || e.Status == EscrowStatus.Frozen);

            if (activeEscrow == null)
            {
                return new RefundBookingDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No Held or Frozen escrow funds found for this booking."
                };
            }

            booking.Status = BookingStatus.ResolvedRefunded; 

            activeEscrow.Status = EscrowStatus.Refunded; 
            activeEscrow.ReleasedAt = DateTime.UtcNow; 

            await context.SaveChangesAsync(cancellationToken);

            // 7. إرسال الإشعارات (إشعار إيجابي للعميل، سلبي للمزود)
            //if (notificationService != null)
            //{
            //    // للعميل (أخبار سعيدة)
            //    await notificationService.SendAsync(
            //        userId: booking.CustomerId,
            //        title: "تم حسم النزاع لصالحك",
            //        message: $"تم حل النزاع للحجز رقم {booking.BookingNumber} وتم توجيه استرداد المبلغ لمحفظتك/بطاقتك."
            //    );

            //    // للمزود (أخبار الإدارة)
            //    await notificationService.SendAsync(
            //        userId: booking.ServiceListing.ProviderProfileId,
            //        title: "قرار الإدارة بشأن النزاع",
            //        message: $"تم إغلاق النزاع للحجز رقم {booking.BookingNumber} برد المبلغ للعميل. ملاحظة الإدارة: {request.DecisionNote}"
            //    );
            //}

            return new RefundBookingDisputeResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                BookingId = booking.Id,
                Status = booking.Status.ToString(),
                StatusAr = "تم رد المبلغ للعميل",
                EscrowStatus = activeEscrow.Status.ToString()
            };
        }
    }
}
