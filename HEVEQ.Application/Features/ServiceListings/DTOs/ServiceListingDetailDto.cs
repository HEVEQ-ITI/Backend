using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public class ServiceListingDetailDto
    {
        public Guid Id { get; set; }
        public Guid ProviderProfileId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Governorate { get; set; }
        public string? Region { get; set; }
        public string? Tags { get; set; }
        public string? EquipmentModel { get; set; }
        public string? EquipmentCapacity { get; set; }
        public EquipmentCondition? EquipmentCondition { get; set; }
        public int? YearOfManufacture { get; set; }
        public string? EquipmentRegistrationNumber { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? DailyRate { get; set; }
        public int MinimumBookingHours { get; set; }
        public ServiceListingStatus Status { get; set; }
        public int? QualityScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ServiceListingPhotoDto> Photos { get; set; } = [];
        public ICollection<ServiceListingOperatorDto> Operators { get; set; } = [];
        public ICollection<ServiceListingAvailabilityDto> Availability { get; set; } = [];
    }

}
