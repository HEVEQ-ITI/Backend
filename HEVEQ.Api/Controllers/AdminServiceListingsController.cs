using HEVEQ.Application.Features.Admin.Command.ApproveServiceListing;
using HEVEQ.Application.Features.Admin.Command.RejectServiceListing;
using HEVEQ.Application.Features.Admin.Query.GetPendingServiceListings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                statusAr = result.StatusAr,
                message = result.Message
            });
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectListing(Guid id, [FromBody] RejectServiceListingCommand command)
        {
            // دمج الـ ID من الرابط داخل الـ Command
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

            // إرجاع الـ JSON المطلوب بالضبط
            return Ok(new
            {
                id = result.Id,
                status = result.Status,
                statusAr = result.StatusAr,
                adminRejectionNote = result.AdminRejectionNote
            });
        }
    }
}
