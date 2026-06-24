using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderProfile
{
    public record GetProviderProfileQuery : IRequest<ProviderProfileDto>;
}
