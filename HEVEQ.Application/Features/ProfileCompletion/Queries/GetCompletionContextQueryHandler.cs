using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ProfileCompletion.DTOs;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Application.Features.ProfileCompletion.Queries.GetCompletionContext;

public class GetCompletionContextQueryHandler
    : IRequestHandler<GetCompletionContextQuery, ProfileCompletionContextDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetCompletionContextQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public async Task<ProfileCompletionContextDto> Handle(
        GetCompletionContextQuery request,
        CancellationToken cancellationToken)
    {
        // Rule: Authenticated only. No sensitive data returned.
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var role = _currentUser.Role
            ?? throw new ForbiddenAccessException("Role claim is missing from token.");

        // UserManager gives PhoneNumberConfirmed without loading nav props
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("ApplicationUser", userId);

        // Check default address separately — UserManager doesn't load nav props
        var hasDefaultAddress = await _context.Addresses
            .AsNoTracking()
            .AnyAsync(a => a.UserId == userId && a.IsDefault, cancellationToken);

        var dto = new ProfileCompletionContextDto
        {
            Role = role,
            PhoneVerified = user.PhoneNumberConfirmed,
            HasDefaultAddress = hasDefaultAddress,
        };

        var missing = new List<string>();

        if (role == "Customer")
        {
            var hasProfile = await _context.CustomerProfiles
                .AsNoTracking()
                .AnyAsync(p => p.UserId == userId, cancellationToken);

            dto.ProfileCompleted = hasProfile;
            dto.ProviderProfileCompleted = null;    // not applicable for customers

            if (!hasProfile) missing.Add("CustomerProfile");
            if (!user.PhoneNumberConfirmed) missing.Add("PhoneVerification");
            if (!hasDefaultAddress) missing.Add("DefaultAddress");
        }
        else if (role == "Provider")
        {
            // Provider profile is complete when CompanyName is filled
            var companyName = await _context.ProviderProfiles
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => p.CompanyName)
                .FirstOrDefaultAsync(cancellationToken);

            bool providerComplete = !string.IsNullOrWhiteSpace(companyName);

            dto.ProfileCompleted = providerComplete;
            dto.ProviderProfileCompleted = providerComplete;

            if (!providerComplete) missing.Add("ProviderProfile");
            if (!user.PhoneNumberConfirmed) missing.Add("PhoneVerification");
        }
        else
        {
            // Employee / Admin — profile is managed by admin, not self-service
            dto.ProfileCompleted = true;
            dto.ProviderProfileCompleted = null;
        }

        dto.MissingRequirements = missing;
        return dto;
    }
}