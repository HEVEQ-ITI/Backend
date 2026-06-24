using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketPlaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetCustomerOrder;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketPlaceOrderById;
using HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetProviderOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers
{
    //[Authorize]
    [Route("api/marketplace-orders")]
    [ApiController]
    public class MarketPlaceOrderController : ControllerBase
    {
        public readonly IMediator _mediator;
        public MarketPlaceOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private Guid CurrentUserId => Guid.Parse("FC6FF724-CED5-468A-6964-08DED0682657");

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateMarketPlaceOrderRequest request, CancellationToken cancellationToken)
        {
            var buyerId = CurrentUserId;
            return Ok(await _mediator.Send(new CreateMarketPlaceOrderCommand(buyerId,request),cancellationToken));


        }
        [HttpGet("my/purchases")]
        //[Authorize]
        public async Task<ActionResult<List<PurchaseOrderDto>>> GetMyPurchases(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerOrderQuery(CurrentUserId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("my/sales")]
        //[Authorize]
        public async Task<ActionResult<List<SaleOrderDto>>> GetMySales(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProviderOrderQuery(CurrentUserId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMarketplaceOrderByIdQuery(CurrentUserId, id), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/seller-confirm")]
        //[Authorize(Roles = "Provider")]
        public async Task<ActionResult<MarketplaceOrderDto>> SellerConfirm(Guid id, CancellationToken cancellationToken)
        {
          Guid providerID= Guid.Parse("8E08BD4A-BCEC-4799-E8FF-08DED07104C1");

        var result = await _mediator.Send(new ConfirmMarketplaceOrderCommand(id, providerID), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/dispatch")]
        //[Authorize(Roles = "Provider")]
        public async Task<ActionResult<MarketplaceOrderDto>> Dispatch(Guid id,[FromBody] DispatchMarketplaceOrderRequest request,CancellationToken cancellationToken)
        {
            Guid providerID = Guid.Parse("8E08BD4A-BCEC-4799-E8FF-08DED07104C1");

            var result = await _mediator.Send(new DispatchMarketplaceOrderCommand(id, providerID, request), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/deliver")]
        //[Authorize(Roles = "Provider")]
        public async Task<ActionResult<MarketplaceOrderDto>> Deliver(Guid id, CancellationToken cancellationToken)
        {
            Guid providerID = Guid.Parse("8E08BD4A-BCEC-4799-E8FF-08DED07104C1");

            var result = await _mediator.Send(new DeliverMarketplaceOrderCommand(id, providerID), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<ActionResult<MarketplaceOrderDto>> Complete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CompleteMarketplaceOrderCommand(id, CurrentUserId), cancellationToken);
            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<ActionResult<MarketplaceOrderDto>> Cancel(
        Guid id,
        [FromBody] CancelMarketplaceOrderRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CancelMarketplaceOrderCommand(id, CurrentUserId, request), cancellationToken);
            return Ok(result);
        }




    }
}
