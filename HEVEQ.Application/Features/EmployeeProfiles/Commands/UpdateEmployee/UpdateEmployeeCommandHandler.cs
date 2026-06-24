using AutoMapper;
using AutoMapper.QueryableExtensions;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.EmployeeProfiles.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, EmployeeProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<EmployeeProfileDto> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        // ── Business Rule 1: employee profile must exist ───────────────────
        var profile = await _context.EmployeeProfiles
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeProfileId, cancellationToken)
            ?? throw new NotFoundException(nameof(EmployeeProfile), request.EmployeeProfileId);

        // ── Business Rule 2: the linked user must have Employee role ───────
        var isEmployee = await _userManager.IsInRoleAsync(profile.User, "Employee");
        if (!isEmployee)
            throw new ForbiddenAccessException("This user is not an Employee.");

        // ── Update ApplicationUser fields ──────────────────────────────────
        profile.User.FirstName = request.FirstName;
        profile.User.LastName = request.LastName;
        profile.User.PhoneNumber = request.PhoneNumber;
        profile.User.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(profile.User);

        // ── Update EmployeeProfile fields ──────────────────────────────────
        profile.Department = request.Department;
        profile.AssignedGovernorate = request.AssignedGovernorate;
        profile.IsAvailableForDispatch = request.IsAvailableForDispatch;
        profile.UpdatedAt = DateTime.UtcNow;

        // TotalVerificationsCompleted and TotalTicketsHandled — never touched here

        await _context.SaveChangesAsync(cancellationToken);

        // ── Return updated DTO ─────────────────────────────────────────────
        var dto = await _context.EmployeeProfiles
            .Include(e => e.User)
            .Where(e => e.Id == profile.Id)
            .ProjectTo<EmployeeProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(EmployeeProfile), profile.Id);
    }
}