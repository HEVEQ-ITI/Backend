using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public class ServiceListingOperatorDto
    {
        public Guid OperatorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int? YearsOfExperience { get; set; }
        public string? Specialization { get; set; }
        public string? LicenseType { get; set; }
    }
}
