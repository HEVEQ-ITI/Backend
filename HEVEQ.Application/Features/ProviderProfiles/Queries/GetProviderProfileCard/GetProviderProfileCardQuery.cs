using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderProfileCard;

public record GetProviderProfileCardQuery(Guid ProviderProfileId)
    : IRequest<ProviderProfileCardDto>;