using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HEVEQ.Infrastructure.Persistence
{
    public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<ProviderProfile> ProviderProfiles => Set<ProviderProfile>();
        public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
        public DbSet<CustomerTrustScoreHistory> CustomerTrustScoreHistory => Set<CustomerTrustScoreHistory>();
        public DbSet<EmployeeProfile> EmployeeProfiles => Set<EmployeeProfile>();
        public DbSet<Operator> Operators => Set<Operator>();

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ServiceListing> ServiceListings => Set<ServiceListing>();
        public DbSet<ServiceListingPhoto> ServiceListingPhotos => Set<ServiceListingPhoto>();
        public DbSet<ServiceListingOperator> ServiceListingOperators => Set<ServiceListingOperator>();
        public DbSet<ServiceListingAvailability> ServiceListingAvailability => Set<ServiceListingAvailability>();
        public DbSet<BlackoutDate> BlackoutDates => Set<BlackoutDate>();

        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingTimeAdjustmentRequest> BookingTimeAdjustmentRequests => Set<BookingTimeAdjustmentRequest>();
        public DbSet<OperatorAssignment> OperatorAssignments => Set<OperatorAssignment>();
        public DbSet<EscrowRecord> EscrowRecords => Set<EscrowRecord>();

        public DbSet<MarketplaceListing> MarketplaceListings => Set<MarketplaceListing>();
        public DbSet<MarketplaceListingPhoto> MarketplaceListingPhotos => Set<MarketplaceListingPhoto>();
        public DbSet<MarketplaceOrder> MarketplaceOrders => Set<MarketplaceOrder>();

        public DbSet<Review> Reviews => Set<Review>();

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
        public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();

        public DbSet<JobCompletionEvidenceForm> JobCompletionEvidenceForms => Set<JobCompletionEvidenceForm>();
        public DbSet<JobCompletionEvidencePhoto> JobCompletionEvidencePhotos => Set<JobCompletionEvidencePhoto>();
        public DbSet<FieldVerificationForm> FieldVerificationForms => Set<FieldVerificationForm>();
        public DbSet<FieldVerificationPhoto> FieldVerificationPhotos => Set<FieldVerificationPhoto>();

        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<ConversationReadReceipt> ConversationReadReceipts => Set<ConversationReadReceipt>();

        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<ProviderIncident> ProviderIncidents => Set<ProviderIncident>();
        public DbSet<PlatformSetting> PlatformSettings => Set<PlatformSetting>();
        public DbSet<SearchQueryLog> SearchQueryLogs => Set<SearchQueryLog>();
        public DbSet<DomainEventQueueItem> DomainEventQueue => Set<DomainEventQueueItem>();
        public DbSet<ProviderTrustScoreHistory> ProviderTrustScoreHistory => Set<ProviderTrustScoreHistory>();
        public DbSet<CategoryPricingAggregate> CategoryPricingAggregates => Set<CategoryPricingAggregate>();
        public DbSet<AiInteractionLog> AiInteractionLogs => Set<AiInteractionLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
