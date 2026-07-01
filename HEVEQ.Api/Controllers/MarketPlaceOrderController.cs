using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketPlaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.OpenMarketplaceOrderDispute;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetCustomerOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceEscrow;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketPlaceOrderById;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceOrderTracking;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetProviderOrder;
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
        public async Task<ActionResult<OrderActionResponse>> SellerConfirm(Guid id, CancellationToken cancellationToken)
        {
        var result = await _mediator.Send(new ConfirmMarketplaceOrderCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/dispatch")]
        public async Task<ActionResult<OrderActionResponse>> Dispatch(Guid id,[FromBody] DispatchMarketplaceOrderRequest request,CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new DispatchMarketplaceOrderCommand(id,request), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/deliver")]
        public async Task<ActionResult<OrderActionResponse>> Deliver(Guid id, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new DeliverMarketplaceOrderCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<ActionResult<OrderActionResponse>> Complete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CompleteMarketplaceOrderCommand(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<ActionResult<OrderActionResponse>> Cancel(
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

        [HttpPost("{id:guid}/dispute")]
        public async Task<ActionResult<OpenDisputeResponse>> OpenDispute(Guid id, [FromBody] OpenDisputeRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new OpenMarketplaceOrderDisputeCommand(id, request), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}/escrow")]
        public async Task<ActionResult<MarketplaceEscrowDto>> GetEscrow(Guid id,CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(
                new GetMarketplaceEscrowQuery(id),
                cancellationToken));
        }
    }
}
