using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Common
{
    public static class OrderNotifier
    {
        public static void Notify(IApplicationDbContext context, Guid userId, string eventType, string title, string? body, Guid orderId)
        {
            context.Notifications.Add(new Notification
            {
                UserId = userId,
                EventType = eventType,
                Title = title,
                Body = body,
                ReferenceId = orderId.ToString(),
                ReferenceType = nameof(MarketplaceOrder),
                Channel = NotificationChannel.InApp,
                IsRead = false,
                SentAt = DateTime.UtcNow
            });
        }
    }
}
