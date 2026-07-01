using HEVEQ.Application.Features.Admin.Command.ApproveServiceListing;
using HEVEQ.Application.Features.Admin.Command.RejectServiceListing;
using HEVEQ.Application.Features.Admin.Query.GetPendingServiceListings;
using HEVEQ.Application.Features.Admin.Query.GetServiceListingReviewDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/service-listings")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminServiceListingsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingListings([FromQuery] GetPendingServiceListingsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveListing(Guid id)
        {
            var command = new ApproveServiceListingCommand { Id = id };
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
                status = result.Status,
                statusText = result.StatusText,
                statusAr = result.StatusAr,
                message = result.Message
            });
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectListing(Guid id, [FromBody] RejectServiceListingCommand command)
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminIdString) || !Guid.TryParse(adminIdString, out Guid adminIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            command.Id = id;
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
                status = result.Status,
                statusText = result.StatusText,
                statusAr = result.StatusAr,
                adminRejectionNote = result.AdminRejectionNote
            });
        }


        [HttpGet("{id}/review-details")]
        public async Task<IActionResult> GetReviewDetails(Guid id)
        {
            var query = new GetServiceListingReviewDetailsQuery { Id = id };

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Service listing not found." });
            }

            return Ok(result);
        }
    }
}
