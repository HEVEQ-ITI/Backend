using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.UpdateUserStatus
{
    public class UpdateUserStatusCommandHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<UpdateUserStatusCommand, UpdateUserStatusDTO>
    {
        public async Task<UpdateUserStatusDTO> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.TargetUserId == request.AdminId)
            {
                return new UpdateUserStatusDTO
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "You cannot change your own account status."
                };
            }

            var targetUser = await userManager.Users
                .FirstOrDefaultAsync(u => u.Id == request.TargetUserId, cancellationToken);

            if (targetUser == null)
            {
                return new UpdateUserStatusDTO { IsSuccess = false, StatusCode = 404, Message = "User not found." };
            }

            targetUser.IsActive = request.IsActive;


            var result = await userManager.UpdateAsync(targetUser);

            if (!result.Succeeded)
            {
                return new UpdateUserStatusDTO { IsSuccess = false, StatusCode = 500, Message = "Failed to update user status." };
            }

            return new UpdateUserStatusDTO
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = targetUser.Id,
                IsActive = targetUser.IsActive,
                StatusText = targetUser.IsActive ? "Active" : "Suspended",
                StatusAr = targetUser.IsActive ? "نشط" : "موقوف",
                Message = "User status updated successfully"
            };
        }
    }
}
