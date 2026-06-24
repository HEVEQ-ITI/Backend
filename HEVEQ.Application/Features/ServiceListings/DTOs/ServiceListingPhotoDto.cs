using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public class ServiceListingPhotoDto
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

}
