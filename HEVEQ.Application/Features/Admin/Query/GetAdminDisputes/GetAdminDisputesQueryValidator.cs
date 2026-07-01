using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminDisputes
{
    public class GetAdminDisputesQueryValidator : AbstractValidator<GetAdminDisputesQuery>
    {
        public GetAdminDisputesQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
