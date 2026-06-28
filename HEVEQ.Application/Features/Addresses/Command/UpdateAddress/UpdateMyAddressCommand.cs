using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Addresses.Command.UpdateAddress
{
    public class UpdateMyAddressCommand : IRequest<UpdateAddressDTO>
    {
        [JsonIgnore] // يأتي من الرابط (Route)
        public Guid Id { get; set; }

        [JsonIgnore] // يأتي من التوكن (أمان)
        public Guid UserId { get; set; }

        public string Label { get; set; }
        public string Governorate { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public bool IsDefault { get; set; }
    }
   
}
