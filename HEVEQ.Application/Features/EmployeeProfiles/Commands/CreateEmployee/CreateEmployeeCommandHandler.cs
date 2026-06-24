using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Application.Features.EmployeeProfiles.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, EmployeeProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<EmployeeProfileDto> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        // ── Business Rule 1: email must be unique ──────────────────────────
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("This email is already in use.");

        // ── Business Rule 2: username must be unique ───────────────────────
        if (await _userManager.FindByNameAsync(request.UserName) is not null)
            throw new InvalidOperationException("This username is already taken.");

        // ── Create ApplicationUser ─────────────────────────────────────────
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        // ── Assign Employee role ───────────────────────────────────────────
        const string employeeRole = "Employee";
        if (!await _roleManager.RoleExistsAsync(employeeRole))
            await _roleManager.CreateAsync(new IdentityRole<Guid>(employeeRole));

        await _userManager.AddToRoleAsync(user, employeeRole);

        // ── Auto-generate EmployeeCode (format: EMP-YYYYMMDD-XXXX) ─────────
        var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomPart = Guid.NewGuid().ToString("N")[..4].ToUpper();
        var employeeCode = $"EMP-{datePart}-{randomPart}";

        // ── Create EmployeeProfile ─────────────────────────────────────────
        var profile = new EmployeeProfile
        {
            UserId = user.Id,
            EmployeeCode = employeeCode,
            Department = request.Department,
            AssignedGovernorate = request.AssignedGovernorate,
            IsAvailableForDispatch = request.IsAvailableForDispatch,
            TotalVerificationsCompleted = 0,
            TotalTicketsHandled = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.EmployeeProfiles.AddAsync(profile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // ── Return fresh DTO with User included ────────────────────────────
        var dto = await _context.EmployeeProfiles
            .Include(e => e.User)
            .Where(e => e.Id == profile.Id)
            .ProjectTo<EmployeeProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(EmployeeProfile), profile.Id);
    }
}