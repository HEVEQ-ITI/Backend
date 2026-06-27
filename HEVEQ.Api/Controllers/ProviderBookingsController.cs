using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.Queries.GetProviderActiveJobs;
using HEVEQ.Application.Features.Bookings.Queries.GetProviderBookingRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/provider/bookings")]
    [ApiController]
    [Authorize(Roles = "Provider")]
    public class ProviderBookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;
        public ProviderBookingsController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }
        private Guid GetCurrentUserId()
        {
            if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
                throw new UnauthorizedAccessException("User is not authenticated.");
            return _currentUser.UserId.Value;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetBookingRequests(CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var response = await _mediator.Send(new GetProviderBookingRequestsQuery(providerUserId),cancellationToken);
            return Ok(response);
        }

        [HttpGet("active-jobs")]
        public async Task<IActionResult> GetActiveJobs(CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var response = await _mediator.Send(new GetProviderActiveJobsQuery(providerUserId), cancellationToken);
            return Ok(response);
        }
    }
}
