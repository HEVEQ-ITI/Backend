using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlaceOrders.Common;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceOrderTracking
{
    public class GetMarketplaceOrderTrackingQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<GetMarketplaceOrderTrackingQuery, OrderTrackingDto>
    {
        // Mirrors exactly what CancelMarketplaceOrderCommandHandler treats as cancellable.
        private static readonly MarketplaceOrderStatus[] CancellableStatuses =
        {
            MarketplaceOrderStatus.PendingPayment,
            MarketplaceOrderStatus.PaymentCaptured,
            MarketplaceOrderStatus.SellerConfirmed,
            MarketplaceOrderStatus.Dispatched,
            MarketplaceOrderStatus.Delivered
        };

        private static readonly MarketplaceOrderStatus[] DisputableStatuses =
       {
            MarketplaceOrderStatus.Dispatched,
            MarketplaceOrderStatus.Delivered
        };

        public async Task<OrderTrackingDto> Handle(GetMarketplaceOrderTrackingQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
               .AsNoTracking()
               .Include(o => o.Buyer)
               .Include(o => o.Listing).ThenInclude(l => l.Seller).ThenInclude(s => s.ProviderProfile)
               .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
               ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            var userId = currentUser.UserId.Value;
            var isBuyer = order.BuyerId == userId;
            var isSeller = order.Listing.SellerId == userId;
            var isAdmin = currentUser.Role == "Admin";


            if (!isBuyer && !isSeller && !isAdmin)
                throw new ForbiddenAccessException("You are not allowed to view this order's tracking.");

            var escrowStatus = await context.EscrowRecords
               .Where(e => e.MarketplaceOrderId == order.Id)
               .Select(e => (EscrowStatus?)e.Status)
               .FirstOrDefaultAsync(cancellationToken);


            var timeline = new List<OrderTrackingTimelineItemDto>
            {
                new("Order Created", "تم إنشاء الطلب", order.CreatedAt, true),
                new("Seller Confirmed", "تم تأكيد البائع", order.SellerConfirmedAt, order.SellerConfirmedAt.HasValue),
                new("Dispatched", "تم الشحن", order.DispatchedAt, order.DispatchedAt.HasValue),
                new("Delivered", "تم التوصيل", order.DeliveredAt, order.DeliveredAt.HasValue),
                new("Completed", "مكتمل", order.ConfirmedByBuyerAt, order.ConfirmedByBuyerAt.HasValue)
            };

            if (order.CancelledAt.HasValue)
                timeline.Add(new OrderTrackingTimelineItemDto("Cancelled", "تم الإلغاء", order.CancelledAt, true));


            // Each flag is the exact precondition the matching action handler enforces —
            // ConfirmMarketplaceOrderCommandHandler requires PaymentCaptured, etc.
            var actions = new OrderTrackingActionsDto
            {
                CanSellerConfirm = isSeller && order.Status == MarketplaceOrderStatus.PaymentCaptured,
                CanDispatch = isSeller && order.Status == MarketplaceOrderStatus.SellerConfirmed,
                CanMarkDelivered = isSeller && order.Status == MarketplaceOrderStatus.Dispatched,
                CanComplete = isBuyer && order.Status == MarketplaceOrderStatus.Delivered,
                CanCancel = (isBuyer || isSeller) && CancellableStatuses.Contains(order.Status),
                CanDispute = (isBuyer || isSeller) && DisputableStatuses.Contains(order.Status)
            };
            var seller = order.Listing.Seller;
            var sellerName = seller.ProviderProfile != null && !string.IsNullOrEmpty(seller.ProviderProfile.CompanyName)
                ? seller.ProviderProfile.CompanyName
                : $"{seller.FirstName} {seller.LastName}";

            return new OrderTrackingDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                ListingTitle = order.Listing.Title,
                BuyerName = $"{order.Buyer.FirstName} {order.Buyer.LastName}",
                SellerName = sellerName,
                Amount = order.Amount,
                DeliveryPreference = order.DeliveryPreference.HasValue ? order.DeliveryPreference.Value.ToString() : null,
                TrackingNumber = order.TrackingNumber,
                Status = order.Status.ToString(),
                StatusAr = order.Status.ToArabic(),
                EscrowStatus = escrowStatus?.ToString(),
                Timeline = timeline,
                AvailableActions = actions
            };
        }


    }
 }

