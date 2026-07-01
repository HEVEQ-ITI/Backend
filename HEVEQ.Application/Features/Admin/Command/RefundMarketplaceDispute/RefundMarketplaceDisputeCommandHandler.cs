using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RefundMarketplaceDispute
{
    public class RefundMarketplaceDisputeCommandHandler(
        IApplicationDbContext context)
        : IRequestHandler<RefundMarketplaceDisputeCommand, RefundMarketplaceDisputeResponse>
    {
        public async Task<RefundMarketplaceDisputeResponse> Handle(RefundMarketplaceDisputeCommand request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
                .Include(o => o.EscrowRecords)
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
            {
                return new RefundMarketplaceDisputeResponse { IsSuccess = false, StatusCode = 404, Message = "Marketplace order not found." };
            }

            if (order.Status != MarketplaceOrderStatus.Disputed)
            {
                return new RefundMarketplaceDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot refund dispute. Current order status is {order.Status}."
                };
            }


            var activeEscrow = order.EscrowRecords
                .FirstOrDefault(e => e.Status == EscrowStatus.Held || e.Status == EscrowStatus.Frozen);

            if (activeEscrow == null)
            {
                return new RefundMarketplaceDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No Held or Frozen escrow funds found for this order."
                };
            }

            order.Status = MarketplaceOrderStatus.Completed;

            activeEscrow.Status = EscrowStatus.Refunded;
            activeEscrow.ReleasedAt = DateTime.UtcNow;


            await context.SaveChangesAsync(cancellationToken);

            // 7. إرسال الإشعارات
            //if (notificationService != null)
            //{
            //    // للمشتري (العميل) - أخبار سعيدة
            //    await notificationService.SendAsync(
            //        userId: order.BuyerId,
            //        title: "تم حسم نزاع السوق لصالحك",
            //        message: $"تم حل النزاع للطلب رقم {order.Id.ToString().Substring(0, 8).ToUpper()} وسيتم توجيه استرداد المبلغ لمحفظتك/بطاقتك."
            //    );

            //    // للبائع (المزود) - أخبار الإدارة
            //    await notificationService.SendAsync(
            //        userId: order.Listing.SellerId,
            //        title: "قرار الإدارة بشأن نزاع السوق",
            //        message: $"تم إغلاق النزاع للطلب رقم {order.Id.ToString().Substring(0, 8).ToUpper()} برد المبلغ للمشتري. ملاحظة الإدارة: {request.DecisionNote}"
            //    );
            //}

            // 8. إرجاع النتيجة للواجهة
            return new RefundMarketplaceDisputeResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                OrderId = order.Id,
                Status = order.Status.ToString(),
                StatusAr = "تم رد المبلغ للمشتري",
                EscrowStatus = activeEscrow.Status.ToString() // يتوافق مع المطلوب
            };
        }
    }
}