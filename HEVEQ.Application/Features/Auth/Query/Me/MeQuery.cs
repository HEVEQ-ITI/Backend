using HEVEQ.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Query.Me
{
    public record MeQuery(Guid userId) : IRequest<GetMeResponse>;
   
}
