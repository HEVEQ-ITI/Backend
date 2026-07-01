using HEVEQ.Application.Features.Admin.Command.AddTicketMessage;
using HEVEQ.Application.Features.Admin.Command.ResolveTicket;
using HEVEQ.Application.Features.Admin.Query.GetAdminDisputes;
using HEVEQ.Application.Features.Admin.Query.GetAdminTicketDetails;
using HEVEQ.Application.Features.Admin.Query.GetAdminTickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/tickets")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminTicketsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] GetAdminTicketsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketDetails(Guid id)
        {
            var query = new GetAdminTicketDetailsQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Ticket not found." });
            }
            return Ok(result);
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> AddMessage(Guid id, [FromBody] AddTicketMessageCommand command)
        {
            var adminIdString = User.FindFirstValue("uid");
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.TicketId = id;

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
        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> ResolveTicket(Guid id, [FromBody] ResolveTicketCommand command)
        {
            command.Id = id;
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

            return Ok(result);
        }

    }
}
