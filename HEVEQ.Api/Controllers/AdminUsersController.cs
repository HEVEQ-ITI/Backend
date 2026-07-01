using HEVEQ.Application.Features.Admin.Command.UpdateUserStatus;
using HEVEQ.Application.Features.Admin.Query.GetAdminUsers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUsersController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] GetAdminUsersQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }


        [HttpPatch("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusCommand command)
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminIdString) || !Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            command.TargetUserId = id;
            command.AdminId = adminIdGuid;

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    400 => BadRequest(new { message = result.Message }),
                    404 => NotFound(new { message = result.Message }),
                    _ => StatusCode(500, new { message = result.Message })
                };
            }

            return Ok(new
            {
                id = result.Id,
                isActive = result.IsActive,
                statusText = result.StatusText,
                statusAr = result.StatusAr,
                message = result.Message
            });
        }
    }
}
