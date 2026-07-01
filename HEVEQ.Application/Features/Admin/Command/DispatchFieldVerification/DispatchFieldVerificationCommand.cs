using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.DispatchFieldVerification
{
    public class DispatchFieldVerificationCommand : IRequest<DispatchFieldVerificationResponse>
    {
        public Guid BookingId { get; set; }
        public Guid EmployeeUserId { get; set; }
        public string DispatchInstructions { get; set; }
        [JsonIgnore] public Guid AdminId { get; set; }
    }
}
