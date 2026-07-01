using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminTickets
{
    public class GetAdminTicketsQueryValidator
        : AbstractValidator<GetAdminTicketsQuery>
    {
        public GetAdminTicketsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
