using HEVEQ.Application.Features.Documents.Commands.ApproveDocument;
using HEVEQ.Application.Features.Documents.Commands.RejectDocument;
using HEVEQ.Application.Features.Documents.Commands.UploadDocument;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Application.Features.Documents.Queries.GetMyDocuments;
using HEVEQ.Application.Features.Documents.Queries.GetPendingDocuments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DocumentsController(IMediator mediator) : ControllerBase
    {
        //private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private Guid CurrentUserId => Guid.Parse("FC6FF724-CED5-468A-6964-08DED0682657");

        [HttpPost("api/documents")]
        public async Task<ActionResult<DocumentDto>> Upload(
        [FromBody] UploadDocumentRequest request,
        CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UploadDocumentCommand(CurrentUserId, request), cancellationToken);
            return CreatedAtAction(nameof(GetMy), result);
        }

        [HttpGet("api/documents/my")]
        public async Task<ActionResult<List<DocumentDto>>> GetMy(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetMyDocumentsQuery(CurrentUserId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("api/admin/documents")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<DocumentDto>>> GetPending(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPendingDocumentsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost("api/admin/documents/{id:guid}/approve")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<DocumentDto>> Approve(Guid id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ApproveDocumentCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("api/admin/documents/{id:guid}/reject")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<DocumentDto>> Reject(
            Guid id,
            [FromBody] RejectDocumentRequest request,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RejectDocumentCommand(id, request), cancellationToken);
            return Ok(result);
        }
    }
}
