using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Application.Features.CustomerProfiles.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandHandler
    : IRequestHandler<UpdateCustomerProfileCommand, CustomerProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateCustomerProfileCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<CustomerProfileDto> Handle(
        UpdateCustomerProfileCommand request,
        CancellationToken cancellationToken)
    {
        // ───────── 1. Auth check ─────────
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // ───────── 2. Get user ─────────
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        // ───────── 3. Email check only if changed ─────────
        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists is not null)
                throw new InvalidOperationException("This email is already in use.");
        }

        // ───────── 4. Update Identity fields ─────────
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        // IMPORTANT: use Identity APIs for email
        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailResult = await _userManager.SetEmailAsync(user, request.Email);

            if (!emailResult.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", emailResult.Errors.Select(e => e.Description)));

            await _userManager.SetUserNameAsync(user, request.Email);
        }
        else
        {
            await _userManager.UpdateAsync(user);
        }

        // ───────── 5. Update Address (optional) ─────────
        if (request.DefaultAddress is not null)
        {
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(
                    a => a.UserId == userId && a.IsDefault,
                    cancellationToken);

            if (existingAddress is not null)
            {
                existingAddress.Label = request.DefaultAddress.Label;
                existingAddress.Governorate = request.DefaultAddress.Governorate;
                existingAddress.District = request.DefaultAddress.District;
                existingAddress.Street = request.DefaultAddress.Street;
            }
            else
            {
                await _context.Addresses.AddAsync(new Address
                {
                    UserId = userId,
                    IsDefault = true,
                    Label = request.DefaultAddress.Label,
                    Governorate = request.DefaultAddress.Governorate,
                    District = request.DefaultAddress.District,
                    Street = request.DefaultAddress.Street,
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
            }
        }

        // ───────── 6. Save DB ONCE (important fix) ─────────
        await _context.SaveChangesAsync(cancellationToken);

        // ───────── 7. Return updated profile ─────────
        var dto = await _context.CustomerProfiles
            .Include(p => p.User)
                .ThenInclude(u => u.Addresses)
            .Where(p => p.UserId == userId)
            .ProjectTo<CustomerProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(CustomerProfile), userId);
    }
}