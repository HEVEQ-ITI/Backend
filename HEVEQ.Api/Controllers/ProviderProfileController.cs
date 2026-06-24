using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HEVEQ.Application.Features.ProviderProfiles.Commands.UpdateProviderProfile;
using HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderProfile;
using HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderTrustHistory;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/provider/profile")]
[Authorize(Roles = "Provider")]
public class ProviderProfileController : ControllerBase
{
    private readonly ISender _mediator;

    public ProviderProfileController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProviderProfileQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(
        [FromBody] UpdateProviderProfileCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("me/trust-history")]
    public async Task<IActionResult> GetTrustHistory(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProviderTrustHistoryQuery(), cancellationToken);
        return Ok(result);
    }
}