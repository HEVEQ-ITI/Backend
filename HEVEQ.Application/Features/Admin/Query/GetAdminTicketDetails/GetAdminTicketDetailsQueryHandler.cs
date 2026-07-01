using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminTicketDetails
{
    public class GetAdminTicketDetailsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetAdminTicketDetailsQuery, AdminTicketDetailsDto>
    {
        public async Task<AdminTicketDetailsDto> Handle(GetAdminTicketDetailsQuery request, CancellationToken cancellationToken)
        {
            var ticketData = await context.Tickets
                .Where(t => t.Id == request.Id)
                .Select(t => new
                {
                    t.Id,
                    t.TicketNumber,
                    t.Subject,
                    Status = t.Status.ToString(),
                    t.SubmittedById, 

                    Messages = t.Messages.OrderBy(m => m.CreatedAt).Select(m => new
                    {
                        m.Id,
                        m.SenderId,
                        m.Body,
                        m.IsInternal,
                        m.CreatedAt
                    }).ToList(),

                    // الروابط (إذا كانت التذكرة مرتبطة بحجز أو طلب)
                    t.BookingId,
                    t.MarketplaceOrderId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (ticketData == null)
            {
                return null;
            }

            // 2. تجميع معرّفات المستخدمين (User Ids) لجلب أسمائهم بكفاءة
            var userIdsToFetch = new HashSet<Guid> { ticketData.SubmittedById };
            foreach (var msg in ticketData.Messages)
            {
                userIdsToFetch.Add(msg.SenderId);
            }

            var usersDict = await userManager.Users
                .Where(u => userIdsToFetch.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);

            // 3. تعريب الحالة
            string statusAr = ticketData.Status switch
            {
                "Open" => "مفتوحة",
                "InProgress" => "قيد المعالجة",
                "Resolved" => "محلولة",
                "Closed" => "مغلقة",
                _ => ticketData.Status
            };

            // 4. تركيب الكائن النهائي (Mapping)
            var result = new AdminTicketDetailsDto
            {
                Id = ticketData.Id,
                TicketNumber = ticketData.TicketNumber,
                Subject = ticketData.Subject,
                Status = ticketData.Status,
                StatusAr = statusAr,

                SubmittedBy = new TicketSubmitterDto
                {
                    Id = ticketData.SubmittedById,
                    DisplayName = usersDict.GetValueOrDefault(ticketData.SubmittedById, "Unknown User")
                },

                Messages = ticketData.Messages.Select(m => new TicketMessageDto
                {
                    Id = m.Id,
                    SenderName = usersDict.GetValueOrDefault(m.SenderId, "System/Admin"),
                    Body = m.Body,
                    IsInternal = m.IsInternal,
                    CreatedAt = m.CreatedAt
                }).ToList()
            };

            if (ticketData.BookingId.HasValue)
            {
                var booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == ticketData.BookingId.Value, cancellationToken);
                if (booking != null)
                {
                    result.LinkedBooking = new LinkedBookingDto { Id = booking.Id, BookingReference = booking.BookingNumber ?? "N/A" };
                }
            }

            if (ticketData.MarketplaceOrderId.HasValue)
            {
                var order = await context.MarketplaceOrders.FirstOrDefaultAsync(o => o.Id == ticketData.MarketplaceOrderId.Value, cancellationToken);
                result.LinkedMarketplaceOrder = new LinkedMarketplaceOrderDto { Id = order.Id ,OrderNumber = order.TrackingNumber};
            }

            return result;
        }
    }
}
