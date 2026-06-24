using HEVEQ.Application.Features.MarketPlace.Commands.AddMarketplaceListingPhoto;
using HEVEQ.Application.Features.MarketPlace.Commands.CreateMarketPlaceListing;
using HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketPlaceListing;
using HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketplaceListingPhoto;
using HEVEQ.Application.Features.MarketPlace.Commands.UpdateMarketplaceListing;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListingById;
using HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListings;
using HEVEQ.Application.Features.MarketPlace.Queries.GetProviderMarketplaceListings;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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



        //private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private Guid CurrentUserId => Guid.Parse("FC6FF724-CED5-468A-6964-08DED0682657");

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MarketPlaceListingDTO>>> GetAll([FromQuery] GetMarketPlaceListingsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<MarketplaceListingDetailsDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var isAuthenticated = User.Identity?.IsAuthenticated == true;

            var query = new GetMarketplaceListingByIdQuery
            {
                Id = id,
                RequestingUserId = isAuthenticated ? CurrentUserId : null,
                IsAdmin = isAuthenticated && User.IsInRole("Admin")
            };

            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet]
        [Route("~/api/provider/marketplace-listings")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MarketPlaceListingDTO>>> GetMyListings([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {


            var query = new GetProviderMarketplaceListingsQuery
            {

                SellerId = CurrentUserId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        //[Authorize(Roles = "Provider")]
        public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateMarketplaceListingRequest request,
        CancellationToken cancellationToken)
        {


            Guid listingId = await _mediator.Send(new CreateMarketPlaceListingCommand(request, CurrentUserId), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = listingId }, listingId);
        }


        [HttpPut("{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMarketplaceListingRequest request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateMarketplaceListingCommand(id, CurrentUserId, request), cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMarketPlaceListingCommand(id, CurrentUserId), cancellationToken);
            return NoContent();
        }

        [HttpPost("{id:guid}/photos")]
        // [Authorize(Roles = "Provider")]
        public async Task<ActionResult<Guid>> AddPhoto(
        Guid id,
        [FromBody] AddMarketplacePhotoRequest request,
        CancellationToken cancellationToken)
        {
            var command = new AddMarketplaceListingPhotoCommand(id, request);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{id:guid}/photos/{photoId:guid}")]
         //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> DeletePhoto([FromRoute] Guid id, [FromRoute] Guid photoId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMarketplaceListingPhotoCommand(id, photoId), cancellationToken);
            return NoContent();
        }
    }
}


