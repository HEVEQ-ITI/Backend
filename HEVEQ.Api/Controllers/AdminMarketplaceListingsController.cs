using HEVEQ.Application.Features.Admin.Command.ApproveMarketplaceListing;
using HEVEQ.Application.Features.Admin.Command.RejectMarketplaceListing;
using HEVEQ.Application.Features.Admin.Query.GetMarketplaceListingReviewDetails;
using HEVEQ.Application.Features.Admin.Query.GetPendingMarketplaceListings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/marketplace-listings")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class AdminMarketplaceListingsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingMarketplaceListings([FromQuery] GetPendingMarketplaceListingsQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveListing(Guid id)
        {
            var command = new ApproveMarketplaceListingCommand { Id = id };

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
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectListing(Guid id, [FromBody] RejectMarketplaceListingCommand command)
        {

            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                    400 => BadRequest(new { message = result.Message }),
                    404 => NotFound(new { message = result.Message }),
                    _ => StatusCode(500, new { message = result.Message })
                };
            }

            return Ok(result);
        }

        [HttpGet("{id}/review-details")]
        public async Task<IActionResult> GetReviewDetails(Guid id)
        {
            var query = new GetMarketplaceListingReviewDetailsQuery { Id = id };

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Marketplace listing not found." });
            }

            return Ok(result);
        }
    }
}
