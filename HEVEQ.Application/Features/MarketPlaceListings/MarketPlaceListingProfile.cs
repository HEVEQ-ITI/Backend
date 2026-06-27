using AutoMapper;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using HEVEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceListings
{
    public class MarketPlaceListingProfile:Profile
    {
        public MarketPlaceListingProfile()
        {
        CreateMap<MarketplaceListingPhoto, MarketplaceListingPhotoDto>();

            CreateMap<MarketplaceListing, ProviderMarketPlaceListingDTO>()
               .IncludeBase<MarketplaceListing, MarketPlaceListingDTO>()
               .ForMember(dest => dest.PhotosCount, opt => opt.MapFrom(src => src.Photos.Count));

            CreateMap<MarketplaceListing, MarketPlaceListingDTO>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                    .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src =>
                        !string.IsNullOrEmpty(src.Seller.FirstName)
                            ? $"{src.Seller.FirstName} {src.Seller.LastName}"
                            : src.Seller.UserName))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.District) ? src.Governorate : $"{src.District}, {src.Governorate}"))
                    .ForMember(dest => dest.CoverPhotoUrl, opt => opt.MapFrom(src =>
                        src.Photos.OrderBy(p => p.DisplayOrder).Select(p => p.PhotoUrl).FirstOrDefault()))
                    .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.ToString()))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                    .ForMember(dest => dest.TransactionMethod, opt => opt.MapFrom(src => src.TransactionMethod.ToString()))
                    .ForMember(dest => dest.StatusAr, opt => opt.MapFrom(src => src.Status.ToArabic()))
                    .ForMember(dest => dest.ConditionAr, opt => opt.MapFrom(src => src.Condition.ToArabic()));

            CreateMap<MarketplaceListing, MarketplaceListingDetailsDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.Seller.FirstName)
                    ? $"{src.Seller.FirstName} {src.Seller.LastName}"
                    : src.Seller.UserName))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.ToString()))
            .ForMember(dest => dest.ConditionAr, opt => opt.MapFrom(src => src.Condition.ToArabic()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.StatusAr, opt => opt.MapFrom(src => src.Status.ToArabic()))
        .ForMember(dest => dest.TransactionMethod, opt => opt.MapFrom(src => src.TransactionMethod.ToString()));
        }

    }
}
