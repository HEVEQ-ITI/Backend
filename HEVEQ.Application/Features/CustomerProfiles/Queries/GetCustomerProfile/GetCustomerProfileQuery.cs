using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.CustomerProfiles.Queries.GetCustomerProfile
{
    public record GetCustomerProfileQuery : IRequest<CustomerProfileDto>;
}
