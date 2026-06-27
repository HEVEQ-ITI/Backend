using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HEVEQ.Application.Features.CustomerDashboard.Queries.GetCustomerDashboardSummary;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/customer/dashboard")]
[Authorize(Roles = "Customer")]
public class CustomerDashboardController : ControllerBase
{
    private readonly ISender _mediator;

    public CustomerDashboardController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerDashboardSummaryQuery(), cancellationToken);
        return Ok(result);
    }
}