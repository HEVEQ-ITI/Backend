using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Commands.CompleteBookingByProvider
{
    public sealed class CompleteBookingByProviderCommandHandler : IRequestHandler<CompleteBookingByProviderCommand, CompleteBookingByProviderResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public CompleteBookingByProviderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompleteBookingByProviderResponseDto> Handle(CompleteBookingByProviderCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.Status != BookingStatus.InProgress)
                throw new InvalidOperationException("Booking can only be completed by provider when it is in progress.");

            var providerProfile = await _context.ProviderProfiles
                .FirstOrDefaultAsync(x => x.UserId == request.ProviderUserId, cancellationToken);

            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            if (booking.ServiceListing.ProviderProfileId != providerProfile.Id)
                throw new InvalidOperationException("Only the owner provider can complete this booking.");

            if (booking.AssignedOperatorId is null)
                throw new InvalidOperationException("Booking does not have an assigned operator.");

            var assignment = await _context.OperatorAssignments
                .FirstOrDefaultAsync(x =>
                    x.BookingId == booking.Id &&
                    x.OperatorId == booking.AssignedOperatorId,
                    cancellationToken);

            if (assignment is null)
                throw new InvalidOperationException("Operator assignment was not found for this booking.");

            if (assignment.Status != OperatorAssignmentStatus.InProgress)
                throw new InvalidOperationException("Operator assignment must be in progress before completing the booking.");

            var now = DateTime.Now;

            var evidenceForm = new JobCompletionEvidenceForm
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                SubmittedByUserId = request.ProviderUserId,
                ProviderNotes = request.ProviderNotes,
                Status = EvidenceFormStatus.Submitted,
                SubmittedAt = now,
                CreatedAt = now
            };

            foreach (var photo in request.Photos.OrderBy(x => x.DisplayOrder))
            {
                evidenceForm.Photos.Add(new JobCompletionEvidencePhoto
                {
                    Id = Guid.NewGuid(),
                    FormId = evidenceForm.Id,
                    PhotoUrl = photo.PhotoUrl,
                    Caption = photo.Caption,
                    DisplayOrder = photo.DisplayOrder,
                    CreatedAt = now
                });
            }

            _context.JobCompletionEvidenceForms.Add(evidenceForm);

            booking.Status = BookingStatus.PendingCustomerConfirmation;
            booking.CompletedMarkedAt = now;

            if (booking.StartedAt is not null)
            {
                var actualDuration = now - booking.StartedAt.Value;
                booking.ActualDurationHours = Math.Round((decimal)actualDuration.TotalHours, 2);
            }

            assignment.Status = OperatorAssignmentStatus.Completed;

            await _context.SaveChangesAsync(cancellationToken);

            return new CompleteBookingByProviderResponseDto
            {
                BookingId = booking.Id,
                Status = booking.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                EvidenceFormId = evidenceForm.Id,
                Message = "Completion submitted successfully"
            };
        }
    }
}