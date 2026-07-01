using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminTicketDetails
{
    public class GetAdminTicketDetailsQuery : IRequest<AdminTicketDetailsDto>
    {
        public Guid Id { get; set; }
    }
}
