using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public sealed record CompletionEvidencePhotoDto(string PhotoUrl, string? Caption, int DisplayOrder);
}
