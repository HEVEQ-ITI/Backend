using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.UpdateUserStatus
{
    public class UpdateUserStatusCommand : IRequest<UpdateUserStatusDTO>
    {
        [JsonIgnore] // يأتي من الرابط (الهدف)
        public Guid TargetUserId { get; set; }

        [JsonIgnore] // يأتي من التوكن (المدير الحالي)
        public Guid AdminId { get; set; }

        // تأتي من الـ Body
        public bool IsActive { get; set; }
        public string Reason { get; set; }
    }
}
