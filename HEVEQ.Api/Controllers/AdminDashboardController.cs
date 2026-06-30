using HEVEQ.Application.Features.Admin.Query.GetDashboardSummary;
using HEVEQ.Application.Features.Admin.Query.GetPendingActions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminDashboardController(IMediator _mediator) : ControllerBase
    {

        [HttpGet("dashboard/summary")]
        public async Task<IActionResult> GetSummary()
        {
            var query = new GetDashboardSummaryQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("pending-actions")]
        public async Task<IActionResult> GetPendingActions([FromQuery] GetPendingActionsQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

    }
}
