using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.DTOs
{
    public class CreateAddressDTO
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public bool IsDefault { get; set; }
    }
}
