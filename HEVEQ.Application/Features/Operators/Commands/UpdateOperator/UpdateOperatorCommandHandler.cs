using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Operators.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Operators.Commands.UpdateOperator;

public class UpdateOperatorCommandHandler
    : IRequestHandler<UpdateOperatorCommand, OperatorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UpdateOperatorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<OperatorDto> Handle(
        UpdateOperatorCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Get current provider's profile id
        var providerProfileId = await _context.ProviderProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new NotFoundException(nameof(ProviderProfile), userId);

        // Load operator
        var op = await _context.Operators
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), request.Id);

        // Business Rule: provider can only edit their OWN operators
        if (op.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException(
                "You do not have permission to edit this operator.");

        op.FullName = request.FullName;
        op.YearsOfExperience = request.YearsOfExperience;
        op.Specialization = request.Specialization;
        op.LicenseType = request.LicenseType;
        op.LicenseNumber = request.LicenseNumber;
        op.LicenseExpiryDate = request.LicenseExpiryDate;
        op.ProfilePhotoUrl = request.ProfilePhotoUrl;
        op.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OperatorDto>(op);
    }
}