using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceEarningsSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Authorize(Roles = "Provider")]
    [Route("api/provider/earnings")]
    [ApiController]
    public class ProviderEarningsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("marketplace-summary")]
        public async Task<ActionResult<MarketplaceEarningsSummaryDto>> GetMarketplaceSummary(
            [FromQuery] GetMarketplaceEarningsSummaryQuery query,
            CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(query, cancellationToken));
        }

    }
}
