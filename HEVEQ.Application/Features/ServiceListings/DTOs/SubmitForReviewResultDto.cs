using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record SubmitForReviewResultDto(
        Guid Id,
        string Status,
        string StatusAr,
        string Message);
}
