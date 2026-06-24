using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HEVEQ.Application.Features.Operators.Commands.CreateOperator;
using HEVEQ.Application.Features.Operators.Commands.DeleteOperator;
using HEVEQ.Application.Features.Operators.Commands.UpdateOperator;
using HEVEQ.Application.Features.Operators.Queries.GetProviderOperators;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/provider/operators")]
[Authorize(Roles = "Provider")]
public class ProviderOperatorsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProviderOperatorsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProviderOperatorsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOperatorCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateOperatorCommand command,
        CancellationToken cancellationToken)
    {
        // Merge route id into command to avoid mismatch
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOperatorCommand(id), cancellationToken);
        return NoContent();
    }
}