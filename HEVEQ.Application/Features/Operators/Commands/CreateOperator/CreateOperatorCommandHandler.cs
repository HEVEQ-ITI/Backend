using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Operators.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Operators.Commands.CreateOperator;

public class CreateOperatorCommandHandler
    : IRequestHandler<CreateOperatorCommand, OperatorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateOperatorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<OperatorDto> Handle(
        CreateOperatorCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Business Rule: operator is always tied to the current provider's profile
        var providerProfileId = await _context.ProviderProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new NotFoundException(nameof(ProviderProfile), userId);

        var op = new Operator
        {
            ProviderProfileId = providerProfileId,   // set from server — never from client
            FullName = request.FullName,
            YearsOfExperience = request.YearsOfExperience,
            Specialization = request.Specialization,
            LicenseType = request.LicenseType,
            LicenseNumber = request.LicenseNumber,
            LicenseExpiryDate = request.LicenseExpiryDate,
            ProfilePhotoUrl = request.ProfilePhotoUrl,
            IsActive = true,                          // always active on creation
            CreatedAt = DateTime.UtcNow
        };

        await _context.Operators.AddAsync(op, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OperatorDto>(op);
    }
}