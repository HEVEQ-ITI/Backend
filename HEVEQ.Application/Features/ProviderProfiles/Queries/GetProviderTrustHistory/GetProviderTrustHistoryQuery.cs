using MediatR;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderTrustHistory;

public record GetProviderTrustHistoryQuery : IRequest<List<ProviderTrustHistoryDto>>;