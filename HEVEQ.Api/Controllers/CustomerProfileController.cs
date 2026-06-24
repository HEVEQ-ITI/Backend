using HEVEQ.Application.Features.CustomerProfiles.Commands.UpdateCustomerProfile;
using HEVEQ.Application.Features.CustomerProfiles.Queries.GetCustomerProfile;
using HEVEQ.Application.Features.CustomerProfiles.Queries.GetCustomerTrustHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/customer-profile")]
[Authorize(Roles = "Customer")]
public class CustomerProfileController : ControllerBase
{
    private readonly ISender _mediator;

    public CustomerProfileController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerProfileQuery(), cancellationToken);
        return Ok(result);
    }
    [HttpGet("me/trust-history")]
    public async Task<IActionResult> GetMyTrustHistory(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerTrustHistoryQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(
    [FromBody] UpdateCustomerProfileCommand command,
    CancellationToken cancellationToken)
    {
       var result= await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}