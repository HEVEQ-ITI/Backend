using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.DTOs
{
    public class AddressDTO
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Governorate { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsDefault { get; set; }
    }
}
