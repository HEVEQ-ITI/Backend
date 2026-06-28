using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class VerificationUserDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
    }
}
