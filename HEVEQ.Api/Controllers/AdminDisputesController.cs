using HEVEQ.Application.Features.Admin.Command.PartialSettleBookingDispute;
using HEVEQ.Application.Features.Admin.Command.RefundBookingDispute;
using HEVEQ.Application.Features.Admin.Command.ReleaseBookingDispute;
using HEVEQ.Application.Features.Admin.Query.GetAdminDisputes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/disputes")]
    [ApiController]
    [Authorize(Roles ="Amdin")]
    public class AdminDisputesController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetDisputes([FromQuery] GetAdminDisputesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("bookings/{bookingId}/release-to-provider")]
        public async Task<IActionResult> ReleaseToProvider(Guid bookingId, [FromBody] ReleaseBookingDisputeCommand command)
        {

            var adminIdString = User.FindFirstValue("uid");
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.BookingId = bookingId;

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


        [HttpPost("bookings/{bookingId}/refund-customer")]
        public async Task<IActionResult> RefundCustomer(Guid bookingId, [FromBody] RefundBookingDisputeCommand command)
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.BookingId = bookingId;

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

        [HttpPost("bookings/{bookingId}/partial-settlement")]
        public async Task<IActionResult> PartialSettlement(Guid bookingId, [FromBody] PartialSettleBookingDisputeCommand command)
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.BookingId = bookingId;

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
