using AutoMapper;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders
{
    public class MarketPlaceOrderProfile:Profile
    {
        public MarketPlaceOrderProfile()
        {
            CreateMap<MarketplaceOrder, MarketplaceOrderDto>()
                 .ForMember(d => d.BuyerName, opt => opt.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"))
                 .ForMember(d => d.SellerId, opt => opt.MapFrom(s => s.Listing.SellerId))
                 .ForMember(d => d.SellerName, opt => opt.MapFrom(s => $"{s.Listing.Seller.FirstName} {s.Listing.Seller.LastName}"))
                 .ForMember(d => d.ListingTitle, opt => opt.MapFrom(s => s.Listing.Title))
                 .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                 .ForMember(d => d.StatusAr, opt => opt.MapFrom(s => s.Status.ToArabic()))
                 .ForMember(d => d.DeliveryPreference, opt => opt.MapFrom(s =>
                     s.DeliveryPreference.HasValue ? s.DeliveryPreference.Value.ToString() : null))
                 .ForMember(d => d.CancellationInitiatedByRole, opt => opt.MapFrom(s =>
                     s.CancellationInitiatedByRole.HasValue ? s.CancellationInitiatedByRole.Value.ToString() : null))
                 .ForMember(d => d.ViewerRole, opt => opt.Ignore());

            CreateMap<MarketplaceOrder, PurchaseOrderDto>()
                .ForMember(d => d.ListingTitle, opt => opt.MapFrom(s => s.Listing.Title))
                .ForMember(d => d.SellerId, opt => opt.MapFrom(s => s.Listing.SellerId))
                .ForMember(d => d.SellerName, opt => opt.MapFrom(s => $"{s.Listing.Seller.FirstName} {s.Listing.Seller.LastName}"))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.StatusAr, opt => opt.MapFrom(s => s.Status.ToArabic()))
                .ForMember(d => d.DeliveryPreference, opt => opt.MapFrom(s =>
                    s.DeliveryPreference.HasValue ? s.DeliveryPreference.Value.ToString() : null))
                .ForMember(d => d.CancellationInitiatedByRole, opt => opt.MapFrom(s =>
                    s.CancellationInitiatedByRole.HasValue ? s.CancellationInitiatedByRole.Value.ToString() : null));

            CreateMap<MarketplaceOrder, SaleOrderDto>()
                .ForMember(d => d.ListingTitle, opt => opt.MapFrom(s => s.Listing.Title))
                .ForMember(d => d.BuyerId, opt => opt.MapFrom(s => s.BuyerId))
                .ForMember(d => d.BuyerName, opt => opt.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.StatusAr, opt => opt.MapFrom(s => s.Status.ToArabic()))
                .ForMember(d => d.DeliveryPreference, opt => opt.MapFrom(s =>
                    s.DeliveryPreference.HasValue ? s.DeliveryPreference.Value.ToString() : null))
                .ForMember(d => d.CancellationInitiatedByRole, opt => opt.MapFrom(s =>
                    s.CancellationInitiatedByRole.HasValue ? s.CancellationInitiatedByRole.Value.ToString() : null));
        }
    }
}
