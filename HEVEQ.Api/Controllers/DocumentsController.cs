using HEVEQ.Application.Features.Documents.Commands.ApproveDocument;
using HEVEQ.Application.Features.Documents.Commands.DeleteDocument;
using HEVEQ.Application.Features.Documents.Commands.RejectDocument;
using HEVEQ.Application.Features.Documents.Commands.UploadDocument;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Application.Features.Documents.Queries.GetMyDocuments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/documents")]
    [ApiController]
    //[Authorize]
    public class DocumentsController(IMediator mediator) : ControllerBase
    {
        //private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private Guid CurrentUserId => Guid.Parse("FC6FF724-CED5-468A-6964-08DED0682657");

        [HttpPost]
        public async Task<ActionResult<DocumentDto>> Upload(
        [FromBody] UploadDocumentRequest request,
        CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UploadDocumentCommand(request), cancellationToken);
            return CreatedAtAction(nameof(GetMy), result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<DocumentDto>>> GetMy(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetMyDocumentsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteDocumentCommand(id), cancellationToken);
            return NoContent();
        }

     

    }
}
