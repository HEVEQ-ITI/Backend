using HEVEQ.Api.Requests.Bookings;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.Commands.AcceptBooking;
using HEVEQ.Application.Features.Bookings.Commands.ApproveTimeAdjustment;
using HEVEQ.Application.Features.Bookings.Commands.CancelBooking;
using HEVEQ.Application.Features.Bookings.Commands.CheckoutBookingPayment;
using HEVEQ.Application.Features.Bookings.Commands.CompleteBookingByProvider;
using HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingCompletion;
using HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingPayment;
using HEVEQ.Application.Features.Bookings.Commands.CreateBooking;
using HEVEQ.Application.Features.Bookings.Commands.CreateTimeAdjustment;
using HEVEQ.Application.Features.Bookings.Commands.DisputeBooking;
using HEVEQ.Application.Features.Bookings.Commands.RejectBooking;
using HEVEQ.Application.Features.Bookings.Commands.RejectTimeAdjustment;
using HEVEQ.Application.Features.Bookings.Commands.StartBooking;
using HEVEQ.Application.Features.Bookings.Queries.GetBookingById;
using HEVEQ.Application.Features.Bookings.Queries.GetBookingCreateContext;
using HEVEQ.Application.Features.Bookings.Queries.GetBookingEscrow;
using HEVEQ.Application.Features.Bookings.Queries.GetBookingTracker;
using HEVEQ.Application.Features.Bookings.Queries.GetCustomerBookings;
using HEVEQ.Application.Features.Bookings.Queries.GetProviderBookings;
using HEVEQ.Api.Requests.Payments;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;
        public BookingsController(IMediator mediator, ICurrentUserService currentUser)
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

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new CreateBookingCommand(
                customerId,
                request.ServiceListingId,
                request.JobTitle,
                request.JobDescription,
                request.AddressId,
                request.Governorate,
                request.District,
                request.Street,
                request.Latitude,
                request.Longitude,
                request.RequestedStartDate,
                request.RequestedStartTime,
                request.EstimatedDurationHours,
                request.SiteContactName,
                request.SiteContactPhone,
                request.AccessRequirements,
                request.SafetyNotes,
                request.AcceptOutOfZoneSurcharge
            );
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/accept")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> AcceptBooking([FromRoute] Guid bookingId, [FromBody] AcceptBookingRequest request, CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var acceptCommand = new AcceptBookingCommand(providerUserId, bookingId, request.OperatorId);
            var booking = await _mediator.Send(acceptCommand, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/reject")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> RejectBooking([FromRoute] Guid bookingId, [FromBody] RejectBookingRequest request, CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var rejectCommand = new RejectBookingCommand(providerUserId, bookingId, request.Reason);
            var response = await _mediator.Send(rejectCommand, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/cancel")]
        [Authorize(Roles = "Customer,Provider")]
        public async Task<IActionResult> CancelBooking([FromRoute] Guid bookingId, [FromBody] CancelBookingRequest request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var cancelCommand = new CancelBookingCommand(userId, bookingId, request.Reason);
            var response = await _mediator.Send(cancelCommand, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/start")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> StartBooking([FromRoute] Guid bookingId, CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var startCommand = new StartBookingCommand(providerUserId, bookingId);
            var response = await _mediator.Send(startCommand, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/complete-by-provider")]
         [Authorize(Roles = "Provider")]
        public async Task<IActionResult> CompleteByProvider([FromRoute] Guid bookingId, [FromBody] CompleteBookingByProviderRequest request, CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var command = new CompleteBookingByProviderCommand(providerUserId, bookingId, request.ProviderNotes, request.Photos);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/confirm-completion")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmCompletion([FromRoute] Guid bookingId, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new ConfirmBookingCompletionCommand(customerId, bookingId);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/dispute")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DisputeBooking([FromRoute] Guid bookingId, [FromBody] DisputeBookingRequest request, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new DisputeBookingCommand(customerId, bookingId, request.Reason, request.EvidencePhotoUrls);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{bookingId:guid}/time-adjustments")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> CreateTimeAdjustment([FromRoute] Guid bookingId, [FromBody] CreateTimeAdjustmentRequest request, CancellationToken cancellationToken)
        {
            var providerUserId = GetCurrentUserId();
            var command = new CreateTimeAdjustmentCommand(providerUserId, bookingId, request.RequestedAdditionalHrs, request.ProviderNote);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("time-adjustments/{id:guid}/approve")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ApproveTimeAdjustment([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new ApproveTimeAdjustmentCommand(customerId, id);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("time-adjustments/{id:guid}/reject")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RejectTimeAdjustment([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new RejectTimeAdjustmentCommand(customerId, id);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }


        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyBookings([FromQuery] BookingStatus? status, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var bookings = await _mediator.Send(new GetCustomerBookingsQuery(customerId, status), cancellationToken);
            return Ok(bookings);
        }

        [HttpGet("provider")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> GetProviderBookings(CancellationToken cancellationToken)
        {
            var providerId = GetCurrentUserId();
            var bookings = await _mediator.Send(new GetProviderBookingsQuery(providerId), cancellationToken);
            return Ok(bookings);
        }

        [HttpGet("create-context/{serviceListingId:guid}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetBookingCreateContext([FromRoute] Guid serviceListingId, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var context = await _mediator.Send(new GetBookingCreateContextQuery(customerId, serviceListingId), cancellationToken);
            return Ok(context);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Customer,Provider,Admin")]
        public async Task<IActionResult> GetBookingById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var booking = await _mediator.Send(new GetBookingByIdQuery(userId, _currentUser.Role, id), cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{id:guid}/payment/checkout")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CheckoutBookingPayment([FromRoute] Guid id, [FromBody] PaymentCheckoutRequest request, CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new CheckoutBookingPaymentCommand(customerId, id, request.PaymentMethod, request.SuccessUrl, request.CancelUrl);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("{id:guid}/payment/mock-confirm")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmBookingPaymentForDemo([FromRoute] Guid id, [FromBody] PaymentConfirmRequest request,  CancellationToken cancellationToken)
        {
            var customerId = GetCurrentUserId();
            var command = new ConfirmBookingPaymentCommand(customerId, id, request.PaymentGatewayReference);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id:guid}/escrow")]
        [Authorize(Roles = "Customer,Provider,Admin")]
        public async Task<IActionResult> GetBookingEscrow([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var command = new GetBookingEscrowQuery(userId, _currentUser.Role, id);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id:guid}/tracker")]
        [Authorize(Roles = "Customer,Provider,Admin")]
        public async Task<IActionResult> GetBookingTracker([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var response = await _mediator.Send(new GetBookingTrackerQuery(userId, _currentUser.Role, id), cancellationToken);
            return Ok(response);
        }
    }
}
