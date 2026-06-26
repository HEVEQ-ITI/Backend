using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Auth.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Query.Me
{
    public class MeQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        : IRequestHandler<MeQuery, GetMeResponse>
    {
        public async Task<GetMeResponse> Handle(MeQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            var user = await userManager.Users
                .AsNoTracking() 
                .FirstOrDefaultAsync(x => x.Id == request.userId, cancellationToken);
            if (user == null)
            {
                return new GetMeResponse { IsSuccess = false, StatusCode = 404, Message = "User not found" };
            }

           
            if (!user.IsActive)
            {
                return new GetMeResponse
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "Your account has been deactivated. Please contact support."
                };
            }
            var roles = await userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? "User";

            // بناء الاستجابة الأساسية (بدون بيانات حساسة)
            var response = new GetMeResponse
            {
                IsSuccess = true,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DisplayName = $"{user.FirstName} {user.LastName}".Trim(),
                Role = primaryRole,
                IsActive = user.IsActive,
                ProfileCompleted = false,
                DashboardUrl = ""
            };
            if (primaryRole == "Provider")
            {
                var provider = await context.ProviderProfiles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

                if (provider != null)
                {
                    response.TrustScore = provider.TrustScore; // بفرض وجود هذا الحقل
                    response.ProfileCompleted =
                        !string.IsNullOrEmpty(provider.CompanyName) &&
                        provider.ServiceRadiusKm > 0; // نفس المنطق الذي ضبطناه مسبقاً
                }
            }
            else if (primaryRole == "Customer")
            {
                var hasAddress = await context.Addresses.AnyAsync(x => x.UserId == user.Id, cancellationToken);
                var hasDocs = await context.Documents.AnyAsync(x => x.UserId == user.Id, cancellationToken);

                response.ProfileCompleted = hasAddress || hasDocs;
            }

            return response;
        }
    }
    
}
