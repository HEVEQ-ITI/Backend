using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderProfile;

public class GetProviderProfileQueryHandler
    : IRequestHandler<GetProviderProfileQuery, ProviderProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetProviderProfileQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<ProviderProfileDto> Handle(
        GetProviderProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Filter by UserId from token — provider can NEVER see another provider's profile
        var dto = await _context.ProviderProfiles
            .Include(p => p.User)
            .Where(p => p.UserId == userId)
            .ProjectTo<ProviderProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(ProviderProfile), userId);
    }
}