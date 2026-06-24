using HEVEQ.Application.Features.ServiceListings.Commands;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Application.Features.ServiceListings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEVEQ.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/service-listings")]
public class ServiceListingsController(ISender sender) : ControllerBase
{
    // ---------- Core (Service Listings) ----------

    [HttpPost]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Guid>> Create(CreateServiceListingRequest request)
    {
        var id = await sender.Send(new CreateServiceListingCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<List<ServiceListingDto>>> GetApproved()
    {
        return await sender.Send(new GetApprovedServiceListingsQuery());
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<ServiceListingDetailDto>> GetById(Guid id)
    {
        return await sender.Send(new GetServiceListingByIdQuery(id));
    }

    [HttpGet("/api/provider/service-listings")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<List<ServiceListingDto>>> GetMine()
    {
        return await sender.Send(new GetProviderServiceListingsQuery());
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> Update(Guid id, UpdateServiceListingRequest request)
    {
        await sender.Send(new UpdateServiceListingCommand(id, request));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await sender.Send(new DeleteServiceListingCommand(id));
        return NoContent();
    }

    // ---------- Photos ----------

    [HttpPost("{id:guid}/photos")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Guid>> AddPhoto(Guid id, AddPhotoRequest request)
    {
        var photoId = await sender.Send(new AddServiceListingPhotoCommand(id, request));
        return Ok(photoId);
    }

    [HttpDelete("{id:guid}/photos/{photoId:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> DeletePhoto(Guid id, Guid photoId)
    {
        await sender.Send(new DeleteServiceListingPhotoCommand(id, photoId));
        return NoContent();
    }

    // ---------- Operators ----------

    [HttpPost("{id:guid}/operators")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> LinkOperator(Guid id, LinkOperatorRequest request)
    {
        await sender.Send(new LinkOperatorToListingCommand(id, request));
        return NoContent();
    }

    [HttpDelete("{id:guid}/operators/{operatorId:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> UnlinkOperator(Guid id, Guid operatorId)
    {
        await sender.Send(new UnlinkOperatorFromListingCommand(id, operatorId));
        return NoContent();
    }

    // ---------- Availability ----------

    [HttpPost("{id:guid}/availability")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Guid>> AddAvailability(Guid id, AddAvailabilityRequest request)
    {
        var availabilityId = await sender.Send(new AddServiceListingAvailabilityCommand(id, request));
        return Ok(availabilityId);
    }

    [HttpPut("{id:guid}/availability/{availabilityId:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> UpdateAvailability(Guid id, Guid availabilityId, UpdateAvailabilityRequest request)
    {
        await sender.Send(new UpdateServiceListingAvailabilityCommand(id, availabilityId, request));
        return NoContent();
    }

    [HttpDelete("{id:guid}/availability/{availabilityId:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> DeleteAvailability(Guid id, Guid availabilityId)
    {
        await sender.Send(new DeleteServiceListingAvailabilityCommand(id, availabilityId));
        return NoContent();
    }

    // ---------- Blackout Dates ----------

    [HttpPost("{id:guid}/blackout-dates")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Guid>> AddBlackoutDate(Guid id, AddBlackoutDateRequest request)
    {
        var blackoutDateId = await sender.Send(new AddBlackoutDateCommand(id, request));
        return Ok(blackoutDateId);
    }

    [HttpDelete("{id:guid}/blackout-dates/{blackoutDateId:guid}")]
    [Authorize(Roles = "Provider")]
    public async Task<IActionResult> DeleteBlackoutDate(Guid id, Guid blackoutDateId)
    {
        await sender.Send(new DeleteBlackoutDateCommand(id, blackoutDateId));
        return NoContent();
    }
}