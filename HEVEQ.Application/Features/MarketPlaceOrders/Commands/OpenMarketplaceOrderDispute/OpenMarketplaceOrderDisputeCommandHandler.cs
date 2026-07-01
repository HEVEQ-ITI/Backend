using HEVEQ.Application.Common.Enums;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Helpers;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.OpenMarketplaceOrderDispute
{
    public class OpenMarketplaceOrderDisputeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<OpenMarketplaceOrderDisputeCommand, OpenDisputeResponse>
    {
        // PaymentCaptured → "I paid but nothing was ever confirmed/sent"
        // Delivered → "Something arrived but was wrong/damaged"
        private static readonly MarketplaceOrderStatus[] DisputableStatuses =
        {
            MarketplaceOrderStatus.PaymentCaptured,
            MarketplaceOrderStatus.Delivered
        };
        public async Task<OpenDisputeResponse> Handle(OpenMarketplaceOrderDisputeCommand command, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var buyerId = currentUser.UserId.Value;
            var request = command.Request;

            var order = await context.MarketplaceOrders
               .Include(o => o.Listing)
               .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken)
               ?? throw new NotFoundException(nameof(MarketplaceOrder), command.OrderId);

            if (order.BuyerId != buyerId)
                throw new ForbiddenAccessException("Only the buyer of this order can open a dispute.");

            if (!DisputableStatuses.Contains(order.Status))
                throw new ValidationException("Status",
                    $"A dispute can only be opened when the order is '{MarketplaceOrderStatus.PaymentCaptured}' or '{MarketplaceOrderStatus.Delivered}'. Current status is '{order.Status}'.");

            // Prevent duplicate disputes on the same order.
            var alreadyDisputed = await context.Tickets
                .AnyAsync(t => t.MarketplaceOrderId == order.Id, cancellationToken);

            if (alreadyDisputed)
                throw new ValidationException("Dispute", "A dispute has already been opened for this order.");

            // Transition order to Disputed.
            order.Status = MarketplaceOrderStatus.Disputed;

            // Freeze escrow if a record exists — protects buyer funds while dispute
            // is under review. Nothing to freeze if no escrow yet (e.g. PaymentCaptured
            // but escrow record not yet created by the payment webhook).
            var escrow = await context.EscrowRecords
                .FirstOrDefaultAsync(e => e.MarketplaceOrderId == order.Id, cancellationToken);

            if (escrow is not null && escrow.Status != EscrowStatus.Frozen)
            {
                escrow.Status = EscrowStatus.Frozen;
                escrow.FrozenAt = DateTime.UtcNow;
                escrow.FreezeReason = $"Dispute opened by buyer: {request.Reason}";
            }

            //TicketService
            // Create the support ticket so Admin team can review.
            var ticket = new Ticket
            {
                TicketNumber = ReferenceNumberGenerator.Generate(ReferenceNumberType.Ticket, Guid.NewGuid()),
                SubmittedById = buyerId,
                MarketplaceOrderId = order.Id,
                Subject = $"Marketplace Dispute — Order {order.OrderNumber}",
                Category = TicketCategory.MarketplaceIssue,
                Priority = 1, // default, admin team can escalate
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Tickets.Add(ticket);

            // Open message with the buyer's reason as the first message.
            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = buyerId,
                Body = request.Reason,
                MessageType = TicketMessageType.User,
                IsInternal = false,
                CreatedAt = DateTime.UtcNow
            };

            context.TicketMessages.Add(ticketMessage);

            // Attach evidence photos.
            if (request.EvidencePhotoUrls?.Count > 0)
            {
                foreach (var url in request.EvidencePhotoUrls)
                {
                    context.TicketAttachments.Add(new TicketAttachment
                    {
                        TicketMessageId = ticketMessage.Id,
                        UploadedByUserId = buyerId,
                        FileUrl = url,
                        FileName = Path.GetFileName(new Uri(url).AbsolutePath),
                        FileType = AttachmentFileType.Image,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
                //NotificationService
                // Notify the seller that a dispute has been opened.
                OrderNotifier.Notify(context, order.Listing.SellerId, "OrderDisputed",
                "Dispute opened", $"A dispute has been opened by the buyer for order {order.OrderNumber}.", order.Id);

                await context.SaveChangesAsync(cancellationToken);

                return new OpenDisputeResponse(
                    order.Id,
                    order.Status.ToString(),
                    order.Status.ToArabic(),
                    ticket.Id,
                    escrow?.Status.ToString() ?? "None",
                    "Dispute opened successfully");
            }
        }
    }

