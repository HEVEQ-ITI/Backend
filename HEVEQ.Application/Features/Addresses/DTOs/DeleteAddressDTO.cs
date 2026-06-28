using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.DTOs
{
    public class DeleteAddressDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Notice { get; set; }
    }
}
