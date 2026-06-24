using MediatR;
using HEVEQ.Application.Features.CustomerProfiles.DTOs;

namespace HEVEQ.Application.Features.CustomerProfiles.Queries.GetCustomerTrustHistory;

public record GetCustomerTrustHistoryQuery : IRequest<List<CustomerTrustHistoryDto>>;


