using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.MarketPlace.Commands.AddMarketplaceListingPhoto;
using HEVEQ.Application.Features.MarketPlace.Commands.CreateMarketPlaceListing;
using HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketPlaceListing;
using HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketplaceListingPhoto;
using HEVEQ.Application.Features.MarketPlace.Commands.UpdateMarketplaceListing;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListingById;
using HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListings;
using HEVEQ.Application.Features.MarketPlace.Queries.GetProviderMarketplaceListings;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/marketplace-listings")]
    [ApiController]
    public class MarketPlaceListingsController : ControllerBase
    {

        public readonly IMediator _mediator;
        public MarketPlaceListingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<MarketPlaceListingDTO>>> GetAll(
            [FromQuery]GetMarketPlaceListingsQuery query,
            CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<MarketplaceListingDetailsDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetMarketplaceListingByIdQuery(id), cancellationToken));
        }

        [HttpGet]
        [Route("~/api/provider/marketplace-listings")]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult<PagedResult<ProviderMarketPlaceListingDTO>>> GetMyListings([FromQuery] GetProviderMarketplaceListingsQuery query, CancellationToken cancellationToken = default)
        {

            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult<CreateMarketplaceListingResponse>> Create(
        [FromBody] CreateMarketplaceListingRequest request,
        CancellationToken cancellationToken)
        {


           var result = await _mediator.Send(new CreateMarketPlaceListingCommand(request), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMarketplaceListingRequest request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateMarketplaceListingCommand(id, request), cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMarketPlaceListingCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpPost("{id:guid}/photos")]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult<Guid>> AddPhoto(
        Guid id,
        [FromBody] AddMarketplacePhotoRequest request,
        CancellationToken cancellationToken)
        {
            var command = new AddMarketplaceListingPhotoCommand(id, request);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{id:guid}/photos/{photoId:guid}")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> DeletePhoto([FromRoute] Guid id, [FromRoute] Guid photoId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMarketplaceListingPhotoCommand(id, photoId), cancellationToken);
            return NoContent();
        }
    }
}


