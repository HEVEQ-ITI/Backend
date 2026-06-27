using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderProfileCard;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/provider")]
[Authorize]
public class ProviderProfileCardController : ControllerBase
{
    private readonly ISender _mediator;

    public ProviderProfileCardController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile-card/{providerProfileId:guid}")]
    public async Task<IActionResult> GetProfileCard(
        Guid providerProfileId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetProviderProfileCardQuery(providerProfileId),
            cancellationToken);

        return Ok(result);
    }
}