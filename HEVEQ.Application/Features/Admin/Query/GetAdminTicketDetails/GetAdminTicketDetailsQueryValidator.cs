using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminTicketDetails
{
    public class GetAdminTicketDetailsQueryValidator : AbstractValidator<GetAdminTicketDetailsQuery>
    {
        public GetAdminTicketDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Ticket ID is required.");
        }
    }
    
}
