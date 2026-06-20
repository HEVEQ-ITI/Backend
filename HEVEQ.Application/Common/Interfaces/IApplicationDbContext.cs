using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using Document = HEVEQ.Domain.Entities.Document;

namespace HEVEQ.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<Address> Addresses { get; }

        DbSet<ProviderProfile> ProviderProfiles { get; }

        DbSet<CustomerProfile> CustomerProfiles { get; }

        DbSet<CustomerTrustScoreHistory> CustomerTrustScoreHistory { get; }

        DbSet<EmployeeProfile> EmployeeProfiles { get; }

        DbSet<Operator> Operators { get; }

        DbSet<Category> Categories { get; }

        DbSet<ServiceListing> ServiceListings { get; }

        DbSet<ServiceListingPhoto> ServiceListingPhotos { get; }

        DbSet<ServiceListingOperator> ServiceListingOperators { get; }

        DbSet<ServiceListingAvailability> ServiceListingAvailability { get; }

        DbSet<BlackoutDate> BlackoutDates { get; }
        DbSet<Document> Documents { get; }
        DbSet<Booking> Bookings { get; }
        DbSet<BookingTimeAdjustmentRequest> BookingTimeAdjustmentRequests { get; }
        DbSet<OperatorAssignment> OperatorAssignments { get; }
        DbSet<EscrowRecord> EscrowRecords { get; }
        DbSet<MarketplaceListing> MarketplaceListings { get; }
        DbSet<MarketplaceListingPhoto> MarketplaceListingPhotos { get; }
        DbSet<MarketplaceOrder> MarketplaceOrders { get; }
        DbSet<Review> Reviews { get; }
        DbSet<Ticket> Tickets { get; }
        DbSet<TicketMessage> TicketMessages { get; }
        DbSet<TicketAttachment> TicketAttachments { get; }
        DbSet<JobCompletionEvidenceForm> JobCompletionEvidenceForms { get; }
        DbSet<JobCompletionEvidencePhoto> JobCompletionEvidencePhotos { get; }
        DbSet<FieldVerificationForm> FieldVerificationForms { get; }
        DbSet<FieldVerificationPhoto> FieldVerificationPhotos { get; }
        DbSet<Conversation> Conversations { get; }
        DbSet<Message> Messages { get; }
        DbSet<ConversationReadReceipt> ConversationReadReceipts { get; }
        DbSet<Notification> Notifications { get; }
        DbSet<ProviderIncident> ProviderIncidents { get; }
        DbSet<PlatformSetting> PlatformSettings { get; }
        DbSet<SearchQueryLog> SearchQueryLogs { get; }
        DbSet<DomainEventQueueItem> DomainEventQueue { get; }
        DbSet<ProviderTrustScoreHistory> ProviderTrustScoreHistory { get; }
        DbSet<CategoryPricingAggregate> CategoryPricingAggregates { get; }
        DbSet<AiInteractionLog> AiInteractionLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
