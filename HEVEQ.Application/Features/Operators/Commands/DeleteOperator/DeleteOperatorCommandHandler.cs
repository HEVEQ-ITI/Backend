using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Operators.Commands.DeleteOperator;

public class DeleteOperatorCommandHandler : IRequestHandler<DeleteOperatorCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteOperatorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeleteOperatorCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var providerProfileId = await _context.ProviderProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new NotFoundException(nameof(ProviderProfile), userId);

        var op = await _context.Operators
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), request.Id);

        // Business Rule: only own operators
        if (op.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException(
                "You do not have permission to delete this operator.");

        // Soft delete — IsActive = false, row stays in DB
        op.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}