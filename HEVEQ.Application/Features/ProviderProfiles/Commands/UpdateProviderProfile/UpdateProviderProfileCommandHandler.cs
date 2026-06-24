using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Application.Features.ProviderProfiles.Commands.UpdateProviderProfile;

public class UpdateProviderProfileCommandHandler
    : IRequestHandler<UpdateProviderProfileCommand, ProviderProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateProviderProfileCommandHandler(
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

    public async Task<ProviderProfileDto> Handle(
        UpdateProviderProfileCommand request,
        CancellationToken cancellationToken)
    {
        // ── Business Rule 1: must be authenticated ────────────────────────
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // ── Business Rule 2: user must exist ──────────────────────────────
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        // ── Business Rule 3: provider profile must exist ──────────────────
        // Always filter by current UserId — provider cannot touch another's profile
        var providerProfile = await _context.ProviderProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken)
            ?? throw new NotFoundException(nameof(ProviderProfile), userId);

        // ── Business Rule 4: email uniqueness ─────────────────────────────
        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailTaken = await _userManager.FindByEmailAsync(request.Email);
            if (emailTaken is not null)
                throw new InvalidOperationException("This email is already in use.");
        }

        // ── Business Rule 5: username uniqueness ──────────────────────────
        if (!string.Equals(user.UserName, request.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var usernameTaken = await _userManager.FindByNameAsync(request.UserName);
            if (usernameTaken is not null)
                throw new InvalidOperationException("This username is already taken.");
        }

        // ── Update ApplicationUser fields ──────────────────────────────────
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            await _userManager.SetEmailAsync(user, request.Email);

        if (!string.Equals(user.UserName, request.UserName, StringComparison.OrdinalIgnoreCase))
            await _userManager.SetUserNameAsync(user, request.UserName);

        if (string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase)
            && string.Equals(user.UserName, request.UserName, StringComparison.OrdinalIgnoreCase))
            await _userManager.UpdateAsync(user);

        // ── Update ProviderProfile fields ──────────────────────────────────
        providerProfile.CompanyName = request.CompanyName;
        providerProfile.BusinessDescription = request.BusinessDescription;
        providerProfile.ServiceRadiusKm = request.ServiceRadiusKm;
        providerProfile.UpdatedAt = DateTime.UtcNow;

        // ── Update location + auto-compute ServiceZoneCenter Point ─────────
        if (request.BaseLatitude.HasValue && request.BaseLongitude.HasValue)
        {
            providerProfile.BaseLatitude = request.BaseLatitude;
            providerProfile.BaseLongitude = request.BaseLongitude;

            // NTS convention: Point(longitude, latitude), SRID 4326 = standard GPS
            providerProfile.ServiceZoneCenter = new Point(
                (double)request.BaseLongitude.Value,
                (double)request.BaseLatitude.Value)
            { SRID = 4326 };
        }

        // TrustScore, AverageRating, ResponseRate — never touched here
        await _context.SaveChangesAsync(cancellationToken);

        // ── Return fresh updated profile ───────────────────────────────────
        var dto = await _context.ProviderProfiles
            .Include(p => p.User)
            .Where(p => p.UserId == userId)
            .ProjectTo<ProviderProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(ProviderProfile), userId);
    }
}