using HEVEQ.Application.Features.ProfileCompletion.Queries.GetCompletionContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileCompletionController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProfileCompletionController(ISender mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("completion-context")]
        public async Task<IActionResult> GetCompletionContext(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCompletionContextQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
