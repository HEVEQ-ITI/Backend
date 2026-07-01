using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.AddTicketMessage
{
    public class AddTicketMessageCommandHandler(
        IApplicationDbContext context) 
        : IRequestHandler<AddTicketMessageCommand, AddTicketMessageResponse>
    {
        public async Task<AddTicketMessageResponse> Handle(AddTicketMessageCommand request, CancellationToken cancellationToken)
        {
            // 1. التأكد من وجود التذكرة
            var ticket = await context.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

            if (ticket == null)
            {
                return new AddTicketMessageResponse { IsSuccess = false, StatusCode = 404, Message = "Ticket not found." };
            }

            
            var message = new TicketMessage
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                SenderId = request.AdminId, 
                IsInternal = request.IsInternal,
                CreatedAt = DateTime.UtcNow
            };

            context.TicketMessages.Add(message);

            ticket.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            // 4. إرسال إشعار ذكي (Smart Notification)
            // إذا لم تكن الرسالة داخلية، نرسل إشعاراً للمستخدم صاحب التذكرة
            //if (!request.IsInternal && notificationService != null)
            //{
            //    await notificationService.SendAsync(
            //        userId: ticket.UserId,
            //        title: "تحديث على تذكرتك",
            //        message: $"تم الرد على تذكرتك رقم {ticket.TicketNumber}. يرجى مراجعة التفاصيل."
            //    );
            //}

            return new AddTicketMessageResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = message.Id,
                Message = "Ticket message added successfully"
            };
        }
    }
}
