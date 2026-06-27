using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListingById;
using HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListings;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/public/service-listings")]
public class PublicServiceListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublicServiceListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PublicPaginatedList<PublicServiceListingDto>>> GetPublicListings(
        [FromQuery] GetPublicServiceListingsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);


    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<PublicServiceListingDetailDto>> GetPublicListingById(Guid id)
    //{
    //    var result = await _mediator.Send(new GetPublicServiceListingByIdQuery(id));

    //    if (result == null)
    //    {
    //        return NotFound(new { message = "The requested active service listing was not found." });
    //    }

    //    return Ok(result);
    //}

    [HttpGet("{id}")]
    public async Task<ActionResult<PublicServiceListingDetailDto>> GetPublicListingById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetPublicServiceListingByIdQuery(id));

            if (result == null)
            {
                return NotFound(new { message = "The requested active service listing was not found." });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "Caught an error directly in Controller!",
                errorType = ex.GetType().Name,
                errorMessage = ex.Message,
                innerError = ex.InnerException?.Message,
                stackTrace = ex.StackTrace
            });
        }
    }
}