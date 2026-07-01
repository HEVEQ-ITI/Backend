using HEVEQ.Application.Features.Admin.Command.UpdateUserStatus;
using HEVEQ.Application.Features.Admin.Query.GetAdminUsers;
using HEVEQ.Application.Features.Admin.Query.GetDashboardSummary;
using HEVEQ.Application.Features.EmployeeProfiles.Commands.CreateEmployee;
using HEVEQ.Application.Features.EmployeeProfiles.Commands.UpdateEmployee;
using HEVEQ.Application.Features.EmployeeProfiles.Queries.GetAllEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/admin/employees")]
[Authorize(Roles = "Admin")]
public class AdminEmployeesController : ControllerBase
{
    private readonly ISender _mediator;

    public AdminEmployeesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        // Merge route id into command — client cannot spoof a different id
        var result = await _mediator.Send(command with { EmployeeProfileId = id }, cancellationToken);
        return Ok(result);
    }    
}