using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public class BlackoutDateDto
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public Guid? OperatorId { get; set; }
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
    }
}
