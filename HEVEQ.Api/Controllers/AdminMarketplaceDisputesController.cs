using HEVEQ.Application.Features.Admin.Command.RefundMarketplaceDispute;
using HEVEQ.Application.Features.Admin.Command.ReleaseMarketplaceDispute;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/disputes/marketplace-orders")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminMarketplaceDisputesController (IMediator _mediator) : ControllerBase
    {

        [HttpPost("{orderId}/release-to-seller")]
        public async Task<IActionResult> ReleaseToSeller(Guid orderId, [FromBody] ReleaseMarketplaceDisputeCommand command)
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.OrderId = orderId;

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

        [HttpPost("{orderId}/refund-buyer")]
        public async Task<IActionResult> RefundBuyer(Guid orderId, [FromBody] RefundMarketplaceDisputeCommand command)
        {

            var adminIdString = User.FindFirstValue("uid");
            if (!string.IsNullOrEmpty(adminIdString) && Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                command.AdminId = adminIdGuid;
            }

            command.OrderId = orderId;

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
