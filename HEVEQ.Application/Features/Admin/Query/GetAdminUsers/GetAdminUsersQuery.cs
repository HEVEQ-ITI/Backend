using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminUsers
{
    public class GetAdminUsersQuery : IRequest<PaginatedUsersResponse>
    {
        public string Role { get; set; }
        public string Status { get; set; }
        public string Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
