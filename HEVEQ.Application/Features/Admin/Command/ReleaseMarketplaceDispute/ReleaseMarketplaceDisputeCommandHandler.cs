using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ReleaseMarketplaceDispute
{
    public class ReleaseMarketplaceDisputeCommandHandler(
        IApplicationDbContext context)
        : IRequestHandler<ReleaseMarketplaceDisputeCommand, ReleaseMarketplaceDisputeResponse>
    {
        public async Task<ReleaseMarketplaceDisputeResponse> Handle(ReleaseMarketplaceDisputeCommand request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
                .Include(o => o.EscrowRecords)
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
            {
                return new ReleaseMarketplaceDisputeResponse { IsSuccess = false, StatusCode = 404, Message = "Marketplace order not found." };
            }

            if (order.Status != MarketplaceOrderStatus.Disputed)
            {
                return new ReleaseMarketplaceDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot resolve dispute. Current order status is {order.Status}."
                };
            }

            var activeEscrow = order.EscrowRecords
                .FirstOrDefault(e => e.Status == EscrowStatus.Held || e.Status == EscrowStatus.Frozen);

            if (activeEscrow == null)
            {
                return new ReleaseMarketplaceDisputeResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No Held or Frozen escrow funds found for this order."
                };
            }

            order.Status = MarketplaceOrderStatus.Completed; 

            activeEscrow.Status = EscrowStatus.Released; 
            activeEscrow.ReleasedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            // 7. إرسال الإشعارات
            //if (notificationService != null)
            //{
            //    // للبائع (تم كسب النزاع)
            //    await notificationService.SendAsync(
            //        userId: order.Listing.SellerId,
            //        title: "تم حسم نزاع السوق لصالحك",
            //        message: $"تم حل النزاع للطلب رقم {order.Id.ToString().Substring(0, 8).ToUpper()} وتم تحويل الأموال لمحفظتك."
            //    );

            //    // للمشتري (تم خسارة النزاع)
            //    await notificationService.SendAsync(
            //        userId: order.BuyerId,
            //        title: "قرار الإدارة بشأن النزاع",
            //        message: $"تم إغلاق النزاع للطلب رقم {order.Id.ToString().Substring(0, 8).ToUpper()} لصالح البائع. ملاحظة الإدارة: {request.DecisionNote}"
            //    );
            //}

            return new ReleaseMarketplaceDisputeResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                OrderId = order.Id,
                Status = order.Status.ToString(),
                StatusAr = "تم الحل لصالح البائع",
                EscrowStatus = activeEscrow.Status.ToString()
            };
        }
    }
}
