using HEVEQ.Application.Features.Admin.Command;
using HEVEQ.Application.Features.Admin.Command.DispatchFieldVerification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/field-verifications")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminFieldVerificationsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("dispatch")]
        public async Task<IActionResult> DispatchVerification([FromBody] DispatchFieldVerificationCommand command)
        {
            var adminIdString = User.FindFirstValue("uid");
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(new { message = result.Message }),
                    400 => BadRequest(new { message = result.Message }),
                    _ => StatusCode(500, new { message = result.Message })
                };
            }

            return Ok(result);
        }

        [HttpPost("{id}/decision")]
        public async Task<IActionResult> SaveDecision(Guid id, [FromBody] FieldVerificationDecisionCommand command)
        {
            var adminIdString = User.FindFirstValue("uid");
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.Id = id;

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(new { message = result.Message }),
                    400 => BadRequest(new { message = result.Message }),
                    _ => StatusCode(500, new { message = result.Message })
                };
            }

            return Ok(result);
        }
    }
}
