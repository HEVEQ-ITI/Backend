using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Addresses.Command.SetDefaultAddress
{
    public class SetDefaultAddressCommand : IRequest<SetDefaultAddressDTO>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
