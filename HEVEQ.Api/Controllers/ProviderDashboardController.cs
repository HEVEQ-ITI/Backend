using HEVEQ.Application.Features.ProviderDashboard.Queries.GetProviderDashboardSummary;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Authorize(Roles = "Provider")]
[Route("api/provider/dashboard")]
public class ProviderDashboardController(ISender sender) : ControllerBase
{
    [HttpGet("summary")]
    [ProducesResponseType(typeof(ProviderDashboardSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderDashboardSummaryDto>> GetSummary()
    {
        var result = await sender.Send(new GetProviderDashboardSummaryQuery());
        return Ok(result);
    }
}