using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.DTOs
{
    public class MarketplaceListingPhotoDto
    {
        public Guid Id { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
}
}
