using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<DeleteAddressDTO>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
