using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.DispatchFieldVerification
{
    public class DispatchFieldVerificationCommandHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<DispatchFieldVerificationCommand, DispatchFieldVerificationResponse>
    {
        public async Task<DispatchFieldVerificationResponse> Handle(DispatchFieldVerificationCommand request, CancellationToken cancellationToken)
        {
            var booking = await context.Bookings
                .Include(b => b.JobCompletionEvidenceForms) 
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                return new DispatchFieldVerificationResponse { IsSuccess = false, StatusCode = 404, Message = "Booking not found." };
            }

            if (booking.Status != BookingStatus.Disputed) 
            {
                return new DispatchFieldVerificationResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot dispatch verification. Booking status must be Disputed, currently is {booking.Status}."
                };
            }

            var employee = await userManager.Users
                .Include(u => u.EmployeeProfile)
                .FirstOrDefaultAsync(u => u.Id == request.EmployeeUserId, cancellationToken);

            if (employee == null)
            {
                return new DispatchFieldVerificationResponse { IsSuccess = false, StatusCode = 404, Message = "Employee not found." };
            }

            var isEmployeeRole = await userManager.IsInRoleAsync(employee, "Employee");
            if (!isEmployeeRole)
            {
                return new DispatchFieldVerificationResponse { IsSuccess = false, StatusCode = 400, Message = "Selected user does not have the Employee role." };
            }

            if (employee.EmployeeProfile == null || !employee.EmployeeProfile.IsAvailableForDispatch)
            {
                return new DispatchFieldVerificationResponse { IsSuccess = false, StatusCode = 400, Message = "Employee is currently not available for dispatch." };
            }

            var linkedEvidence = booking.JobCompletionEvidenceForms.OrderByDescending(e => e.CreatedAt).FirstOrDefault();
            if (linkedEvidence == null)
            {
                return new DispatchFieldVerificationResponse { IsSuccess = false, StatusCode = 400, Message = "No completion evidence found to verify for this booking." };
            }

            var verificationForm = new FieldVerificationForm
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                DispatchedEmployeeId = employee.Id,
                DispatchedByAdminId = request.AdminId, 
                LinkedEvidenceFormId = linkedEvidence.Id, 
                DispatchInstructions = request.DispatchInstructions,
                VisitStatus = VisitStatus.Dispatched,
                DispatchedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            context.FieldVerificationForms.Add(verificationForm);

            booking.Status = BookingStatus.PendingFieldVerification; 
            booking.FieldVerificationDispatchedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            // 7. إرسال إشعار للموظف الميداني
            //if (notificationService != null)
            //{
            //    await notificationService.SendAsync(
            //        userId: employee.Id,
            //        title: "مهمة فحص ميداني جديدة",
            //        message: $"تم إسناد مهمة فحص ميداني للحجز رقم {booking.BookingNumber}. يرجى التوجه للموقع."
            //    );
            //}

            return new DispatchFieldVerificationResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                FieldVerificationFormId = verificationForm.Id,
                BookingId = booking.Id,
                VisitStatus = verificationForm.VisitStatus.ToString(),
                VisitStatusAr = "تم الإرسال",
                Message = "Field verification dispatched successfully"
            };
        }
    }
}
