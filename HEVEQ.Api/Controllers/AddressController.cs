using HEVEQ.Application.Features.Addresses.Query.GetMyAddress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userIdString = User.FindFirstValue("uid");

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            var query = new GetMyAddressQuery(userIdGuid);
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}
