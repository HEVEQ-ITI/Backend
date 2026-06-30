using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetServiceListingReviewDetails
{
    public class GetServiceListingReviewDetailsQuery : IRequest<ServiceListingReviewDetailsDto>
    {
        public Guid Id { get; set; }
    }
}
