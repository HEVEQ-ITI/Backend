using MediatR;
using HEVEQ.Application.Features.Operators.DTOs;

namespace HEVEQ.Application.Features.Operators.Queries.GetProviderOperators;

public record GetProviderOperatorsQuery : IRequest<List<OperatorDto>>;