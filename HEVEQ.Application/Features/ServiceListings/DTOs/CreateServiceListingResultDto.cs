using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{

public class CreateServiceListingResultDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusAr { get; set; } = string.Empty;
    public string NextStep { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
}
