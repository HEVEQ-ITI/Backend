using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAccountVerifications
{
    public class GetAccountVerificationsQuery : IRequest<PaginatedVerificationsResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
    public class PaginatedVerificationsResponse
    {
        public List<PendingVerificationDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
