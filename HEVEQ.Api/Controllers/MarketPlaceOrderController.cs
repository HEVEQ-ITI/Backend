using HEVEQ.Api.Requests.Payments;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketPlaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketplaceOrderPayment;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetCustomerOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketPlaceOrderById;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceOrderTracking;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetProviderOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CheckoutMarketplaceOrderPayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    [Authorize]
    [Route("api/marketplace-orders")]
    [ApiController]
    public class MarketPlaceOrderController : ControllerBase
    {
        public readonly IMediator _mediator;
        public MarketPlaceOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateMarketplaceOrderResponse>> Create(
        [FromBody] CreateMarketPlaceOrderRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new CreateMarketPlaceOrderCommand(request),cancellationToken));
        }

        [HttpGet("my/purchases")]
        public async Task<ActionResult<List<PurchaseOrderDto>>> GetMyPurchases(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerOrderQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("my/sales")]
        public async Task<ActionResult<List<SaleOrderDto>>> GetMySales(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProviderOrderQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMarketplaceOrderByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/seller-confirm")]
        public async Task<ActionResult<MarketplaceOrderDto>> SellerConfirm(Guid id, CancellationToken cancellationToken)
        {
        var result = await _mediator.Send(new ConfirmMarketplaceOrderCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/dispatch")]
        public async Task<ActionResult<MarketplaceOrderDto>> Dispatch(Guid id,[FromBody] DispatchMarketplaceOrderRequest request,CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new DispatchMarketplaceOrderCommand(id,request), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/deliver")]
        public async Task<ActionResult<MarketplaceOrderDto>> Deliver(Guid id, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new DeliverMarketplaceOrderCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<ActionResult<MarketplaceOrderDto>> Complete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CompleteMarketplaceOrderCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<ActionResult<MarketplaceOrderDto>> Cancel(
        Guid id,
        [FromBody] CancelMarketplaceOrderRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CancelMarketplaceOrderCommand(id, request), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}/tracking")]
        public async Task<ActionResult<OrderTrackingDto>> GetTracking(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMarketplaceOrderTrackingQuery(id), cancellationToken);
            return Ok(result);
        }
        [HttpPost("{id:guid}/payment/checkout")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<MarketplaceOrderPaymentCheckoutResponseDto>> CheckoutMarketplaceOrderPayment([FromRoute] Guid id, [FromBody] PaymentCheckoutRequest request, CancellationToken cancellationToken)
        {
            var command = new CheckoutMarketplaceOrderPaymentCommand(id, request.PaymentMethod, request.SuccessUrl, request.CancelUrl);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/payment/mock-confirm")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<MarketplaceOrderPaymentConfirmResponseDto>> ConfirmMarketplaceOrderPaymentForDemo([FromRoute] Guid id, [FromBody] PaymentConfirmRequest request, CancellationToken cancellationToken)
        {
            var command = new ConfirmMarketplaceOrderPaymentCommand(id, request.PaymentGatewayReference);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }


    }
}
