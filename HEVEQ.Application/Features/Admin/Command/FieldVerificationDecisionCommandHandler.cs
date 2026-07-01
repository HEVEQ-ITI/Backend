using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command
{
    public class FieldVerificationDecisionCommandHandler(IApplicationDbContext context)
        : IRequestHandler<FieldVerificationDecisionCommand, FieldVerificationDecisionResponse>
    {
        public async Task<FieldVerificationDecisionResponse> Handle(FieldVerificationDecisionCommand request, CancellationToken cancellationToken)
        {
            var verificationForm = await context.FieldVerificationForms
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (verificationForm == null)
            {
                return new FieldVerificationDecisionResponse { IsSuccess = false, StatusCode = 404, Message = "Field verification form not found." };
            }

            if (verificationForm.VisitStatus != VisitStatus.Completed && verificationForm.VisitStatus != VisitStatus.FailedAccess)
            {
                return new FieldVerificationDecisionResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot make a decision. Visit status must be Completed or FailedAccess, currently is {verificationForm.VisitStatus}."
                };
            }

            if (!Enum.TryParse<FieldVerificationAdminDecision>(request.AdminDecision, true, out var parsedDecision))
            {
                return new FieldVerificationDecisionResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Invalid admin decision type."
                };
            }

            verificationForm.AdminDecision = parsedDecision;
            verificationForm.AdminDecisionNote = request.AdminDecisionNote;

            verificationForm.DecidedByAdminId = request.AdminId;
            verificationForm.DecidedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            string decisionAr = request.AdminDecision switch
            {
                "ReleaseToProvider" => "صرف للمزود",
                "RefundCustomer" => "رد للعميل",
                "PartialSettlement" => "تسوية جزئية",
                _ => request.AdminDecision
            };

            return new FieldVerificationDecisionResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                FieldVerificationFormId = verificationForm.Id,
                AdminDecision = verificationForm.AdminDecision.ToString(),
                AdminDecisionAr = decisionAr,
                Message = "Field verification decision saved successfully"
            };
        }
    }
}
