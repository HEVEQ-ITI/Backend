using HEVEQ.Application.Features.Admin.Query.GetAccountVerifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/admin/account-verifications")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class AdminAccountVerificationsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPendingVerifications([FromQuery] GetAccountVerificationsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
