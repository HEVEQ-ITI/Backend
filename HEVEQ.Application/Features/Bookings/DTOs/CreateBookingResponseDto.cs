using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs;

public class CreateBookingResponseDto
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusAr { get; set; } = string.Empty;
    public string ServiceTitle { get; set; } = string.Empty;
    public string ProviderCompany { get; set; } = string.Empty;
    public DateOnly RequestedStartDate { get; set; }
    public TimeOnly RequestedStartTime { get; set; }
    public decimal EstimatedDurationHours { get; set; }
    public decimal HourlyRateSnapshot { get; set; }
    public decimal EstimatedTotal { get; set; }
    public string Message { get; set; } = string.Empty;
}
