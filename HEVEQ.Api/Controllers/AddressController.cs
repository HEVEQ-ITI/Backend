using HEVEQ.Application.Features.Addresses.Command.AddAddress;
using HEVEQ.Application.Features.Addresses.Command.DeleteAddress;
using HEVEQ.Application.Features.Addresses.Command.SetDefaultAddress;
using HEVEQ.Application.Features.Addresses.Command.UpdateAddress;
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

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressesCommand command)
        {
            var userIdString = User.FindFirstValue("uid");

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }
                        
            command.UserId = userIdGuid;

            var result = await mediator.Send(command);
           
            return Created("", result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] UpdateMyAddressCommand command)
        {
            // 1. استخراج الـ User ID من التوكن
            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            command.Id = id;
            command.UserId = userIdGuid;

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    400 => BadRequest(new { message = result.Message }),
                    403 => StatusCode(403, new { message = result.Message }),
                    404 => NotFound(new { message = result.Message }),
                    _ => BadRequest(new { message = result.Message })
                };
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            // 1. استخراج الـ User ID من التوكن
            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            var command = new DeleteAddressCommand
            {
                Id = id,
                UserId = userIdGuid
            };

            var result = await mediator.Send(command);
            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    403 => StatusCode(403, new { message = result.Message }),
                    404 => NotFound(new { message = result.Message }),
                    _ => BadRequest(new { message = result.Message })
                };
            }

            if (!string.IsNullOrEmpty(result.Notice))
            {
                return Ok(new { message = "Address deleted successfully.", notice = result.Notice });
            }

            return NoContent(); 
        }

        [Authorize]
        [HttpPatch("{id}/set-default")]
        public async Task<IActionResult> SetDefaultAddress(Guid id)
        {

            var userIdString = User.FindFirstValue("uid");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            var command = new SetDefaultAddressCommand
            {
                Id = id,
                UserId = userIdGuid
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    403 => StatusCode(403, new { message = result.Message }),
                    404 => NotFound(new { message = result.Message }),
                    _ => BadRequest(new { message = result.Message })
                };
            }

            return Ok(new
            {
                message = result.Message
            });
        }
    }

}

