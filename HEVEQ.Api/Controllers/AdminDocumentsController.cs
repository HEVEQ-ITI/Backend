using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.Documents.Commands.ApproveDocument;
using HEVEQ.Application.Features.Documents.Commands.RejectDocument;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Application.Features.Documents.Queries.GetAdminDocuments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/documents")]
    [ApiController]
    public class AdminDocumentsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<AdminPagedResult<AdminDocumentDto>>> GetAll(
            [FromQuery] GetAdminDocumentsQuery query,
            CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(query, cancellationToken));
        }


        [HttpPost("{id:guid}/approve")]
        public async Task<ActionResult<ApproveDocumentResponse>> Approve(Guid id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ApproveDocumentCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/reject")]
        public async Task<ActionResult<RejectDocumentResponse>> Reject(
            Guid id,
            [FromBody] RejectDocumentRequest request,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RejectDocumentCommand(id, request), cancellationToken);
            return Ok(result);
        }
    }
}
