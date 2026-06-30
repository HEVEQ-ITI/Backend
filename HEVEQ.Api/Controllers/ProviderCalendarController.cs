using HEVEQ.Application.Features.ProviderCalendar.DTOs;
using HEVEQ.Application.Features.ProviderCalendar.Queries.GetProviderCalendar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Authorize(Roles = "Provider")]
[Route("api/provider/calendar")]
public class ProviderCalendarController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ProviderCalendarResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderCalendarResultDto>> Get([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        var result = await sender.Send(new GetProviderCalendarQuery(from, to));
        return Ok(result);
    }
}