using HEVEQ.Application.Features.ProfileCompletion.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.ProfileCompletion.Queries.GetCompletionContext;

public record GetCompletionContextQuery : IRequest<ProfileCompletionContextDto>;