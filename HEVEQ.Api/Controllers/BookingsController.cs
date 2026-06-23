using HEVEQ.Application.Features.Bookings.Commands.AcceptBooking;
using HEVEQ.Application.Features.Bookings.Commands.ApproveTimeAdjustment;
using HEVEQ.Application.Features.Bookings.Commands.CancelBooking;
using HEVEQ.Application.Features.Bookings.Commands.CompleteBookingByProvider;
using HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingCompletion;
using HEVEQ.Application.Features.Bookings.Commands.CreateBooking;
using HEVEQ.Application.Features.Bookings.Commands.CreateTimeAdjustment;
using HEVEQ.Application.Features.Bookings.Commands.DisputeBooking;
using HEVEQ.Application.Features.Bookings.Commands.RejectBooking;
using HEVEQ.Application.Features.Bookings.Commands.RejectTimeAdjustment;
using HEVEQ.Application.Features.Bookings.Commands.StartBooking;
using HEVEQ.Application.Features.Bookings.Queries.GetBookingById;
using HEVEQ.Application.Features.Bookings.Queries.GetCustomerBookings;
using HEVEQ.Application.Features.Bookings.Queries.GetProviderBookings;
using HEVEQ.Domain.Entities;
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
        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var bookingId = await _mediator.Send(command, cancellationToken);
            return Ok(bookingId);
        }

        [HttpPost("{bookingId:guid}/accept")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> AcceptBooking([FromRoute] Guid bookingId, [FromBody] AcceptBookingCommand command, CancellationToken cancellationToken)
        {
            var acceptCommand = new AcceptBookingCommand(command.ProviderUserId, bookingId, command.OperatorId);
            var booking = await _mediator.Send(acceptCommand, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/reject")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> RejectBooking([FromRoute] Guid bookingId, [FromBody] RejectBookingCommand command, CancellationToken cancellationToken)
        {
            var rejectCommand = new RejectBookingCommand(command.ProviderId, bookingId, command.reason);
            var booking = await _mediator.Send(rejectCommand, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/cancel")]
        //[Authorize(Roles = "Customer,Provider")]
        public async Task<IActionResult> CancelBooking([FromRoute] Guid bookingId, [FromBody] CancelBookingCommand command, CancellationToken cancellationToken)
        {
            var cancelCommand = new CancelBookingCommand(command.UserId, bookingId, command.reason);
            var booking = await _mediator.Send(cancelCommand, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/start")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> StartBooking([FromRoute] Guid bookingId, [FromBody] StartBookingCommand command, CancellationToken cancellationToken)
        {
            var startCommand = new StartBookingCommand(command.ProviderUserId, bookingId);

            var booking = await _mediator.Send(startCommand, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/complete-by-provider")]
        // [Authorize(Roles = "Provider")]
        public async Task<IActionResult> CompleteByProvider([FromRoute] Guid bookingId, CancellationToken cancellationToken)
        {
            var providerUserId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var command = new CompleteBookingByProviderCommand(
                providerUserId,
                bookingId
            );
            var booking = await _mediator.Send(command, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/confirm-completion")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmCompletion([FromRoute] Guid bookingId, CancellationToken cancellationToken)
        {
            var customerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var command = new ConfirmBookingCompletionCommand(
                customerId,
                bookingId);

            var booking = await _mediator.Send(command, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/dispute")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> DisputeBooking([FromRoute] Guid bookingId, CancellationToken cancellationToken)
        {
            var customerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var command = new DisputeBookingCommand(
                customerId,
                bookingId);

            var booking = await _mediator.Send(command, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("{bookingId:guid}/time-adjustments")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> CreateTimeAdjustment([FromRoute] Guid bookingId, [FromBody] CreateTimeAdjustmentCommand command, CancellationToken cancellationToken)
        {
            var createCommand = new CreateTimeAdjustmentCommand(command.ProviderUserId, bookingId, command.AdditionalHours, command.Reason);

            var adjustmentRequestId = await _mediator.Send(createCommand, cancellationToken);
            return Ok(adjustmentRequestId);
        }

        [HttpPost("time-adjustments/{id:guid}/approve")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> ApproveTimeAdjustment([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var command = new ApproveTimeAdjustmentCommand(customerId, id);

            var booking = await _mediator.Send(command, cancellationToken);
            return Ok(booking);
        }

        [HttpPost("time-adjustments/{id:guid}/reject")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> RejectTimeAdjustment([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var command = new RejectTimeAdjustmentCommand(customerId, id);

            var booking = await _mediator.Send(command, cancellationToken);
            return Ok(booking);
        }


        [HttpGet("my")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyBookings(CancellationToken cancellationToken)
        {
            var customerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var bookings = await _mediator.Send(new GetCustomerBookingsQuery(customerId), cancellationToken);
            return Ok(bookings);
        }

        [HttpGet("provider")]
        //[Authorize(Roles = "Provider")]
        public async Task<IActionResult> GetProviderBookings(CancellationToken cancellationToken)
        {
            var providerId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var bookings = await _mediator.Send(new GetProviderBookingsQuery(providerId), cancellationToken);
            return Ok(bookings);
        }


        [HttpGet("{id:guid}")]
        //[Authorize(Roles = "Customer, Provider")]
        public async Task<IActionResult> GetBookingById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var booking = await _mediator.Send(new GetBookingByIdQuery(userId, id), cancellationToken);
            return Ok(booking);
        }
    }
}
