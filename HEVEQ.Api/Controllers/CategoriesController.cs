using HEVEQ.Application.Features.Categories.DTOs;
using HEVEQ.Application.Features.Categories.Queries;
using HEVEQ.Application.Features.Categories.Queries.GetCategories;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEVEQ.Api.Controllers;

[ApiController]
[AllowAnonymous] 
[Route("api/categories")] 
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns the category taxonomy. Optional ?type=Service|Marketplace filter.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryDto>>> GetAllCategories([FromQuery] CategoryType? type)
    {
      
        var categories = await _mediator.Send(new GetCategoriesQuery(type));

        return Ok(categories);
    }
}