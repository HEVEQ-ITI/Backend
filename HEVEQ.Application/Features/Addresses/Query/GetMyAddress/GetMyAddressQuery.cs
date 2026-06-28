using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Query.GetMyAddress
{
    public record GetMyAddressQuery(Guid UserId) : IRequest<List<AddressDTO>>;
    
}
